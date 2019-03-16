# Intent
The project goal is to allow to mock calls to external services in your application by defining a set of reusable patterns and creating libraries for common behavior 
This will allow to deal with the following cases :
- launch end to end test in a mastered environment
- mock a service when it is broken and could take time to be up again (in all environment but production)
- mock a service on all environment but production (for long call time or money reason)

# How it work
- Define a strategy to mock the wanted method :
	* With object
	* With specific behavior
	* Without anything (you may want to not mock between two mocks)
```csharp
using Mock.Dependency.With.Proxy.Define.Strategy; 

var mockMethodStrategy = MockStrategyBuilder
	.ForMethod("method id") // which is used in your proxy to be sure you are in the good place to apply it
    .OnceWithMethodMockStrategy("strategy") // Once or Always | The name of the strategy can be reuse in IOC to load a specific implementation
	.WithContext(new Context(){ ... }); // Not mandatory but if you want to filter on a specific context or get some data to help your implemented mock method to compute a result, fell free to do it
```
- Store it in a database shared with your project
```csharp
using Mock.Dependency.With.Proxy.Define.Strategy;

var repository = new MockStrategyRepositorySqlServer("the connectionString");
repository.MockMethod(mockMethodStrategy);
```
- Get data from the database, apply it and delete it
```csharp
using Mock.Dependency.With.Proxy.Apply.Strategy;
using Mock.Dependency.With.Proxy.Data.Transfer.Objects.Strategies;

var mockStrategy = mockStrategyQuery.GetMockStrategy("method id", s =>
            {
                bool inWantedContext = true;
                s.Context.MatchSome(c =>
                {
                    var context = c as GetMockContext;
                    if (context.SessionId != null && context.SessionId != ApplicationDatabase.SessionId)
                        inWantedContext = false;
                });
                return inWantedContext;
            });

//NoMockStrategy is the default if there is no defined strategy
if (mockStrategy is NoMockStrategy || mockStrategy is ForceNoMockStrategy)
{
    returnedValue = service.Get();
}
else if (mockStrategy is MethodToMockWithObjectStrategy<int> objectStrategy)
{
    returnedValue = objectStrategy.MockedObject;
}
else if (mockStrategy is MethodToMockWithMethodStrategy methodStrategy)
{
	var serviceSubstitute = Container.Resolve<ServiceGetTemplate>(methodStrategy.MethodMockStrategy);
    returnedValue = serviceSubstitute.Get();
}

mockStrategyQuery.RemoveStrategy(mockStrategy); //it don't remove if strategy should always be applied
```

# Todo list

## Functionalities
- Allow to desactivate mock
- Find an elegant way to get the context in a class used as strategy (to define a specific behavior)
- Generate proxy dynamicly
- Create a "mock only on error" strategy
- Add some analyse method to be sure that all call has been used and not more or less for end to end test purpose
- Not have to use method id. Look for NSubstitute like behavior with DynamicProxy like "myService.Get(Arg.Any()).Return(...)""
- Create a view to allow mock on demand

## Examples
- With context that help calculation in a mocked method
- With parameter base strategy
- With multiple methods for the main class
- With builder for object strategy
