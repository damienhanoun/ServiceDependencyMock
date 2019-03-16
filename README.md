# Intent
The project goal is to allow to mock calls to external services in your application by defining a set of reusable patterns and creating libraries for common behavior 

This will allow to deal with the following cases :
- launch end to end test in a mastered environment
- mock a service when it is broken and could take time to be up again (in all environment but production)
- mock a service on all environment but production (for long call time or money reason)

# How it work
## Define strategy part
- Define a strategy to mock the wanted method :
```csharp
using Mock.Dependency.With.Proxy.Define.Strategy; 
```
	* With object
```csharp
var mockStrategy = MockStrategyBuilder.ForMethod("method id")
	.OnceWithObject(yourObject);
```
	* With specific behavior
```csharp
var mockStrategy = MockStrategyBuilder.ForMethod("method id")
	.OnceWithSubstituteBehavior("strategy")
```
	* Without anything (you may want to not mock between two mocks)
```csharp
var mockStrategy = MockStrategyBuilder.ForMethod("method id")
    .OnceWithoutMock();
```
	* Eventually with a context to apply only when needed or specify data to help to generate return value for substitute behavior
```csharp
var mockStrategy = MockStrategyBuilder.ForMethod("method id").OnceWithoutMock();
	.WithContext(new Context(){ ... })
```
- Store it in a database shared with your project
```csharp
var repository = new MockStrategyRepositorySqlServer("the connectionString");
repository.MockMethod(mockMethodStrategy);
```

## Apply strategy part
- Get data from the database
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
```
- Apply it
```csharp
//NoMockStrategy is the default if there is no defined strategy
if (mockStrategy is NoMockStrategy || mockStrategy is ForceNoMockStrategy)
{
    returnedValue = service.Get();
}
else if (mockStrategy is ObjectStrategy<int> objectStrategy)
{
    returnedValue = objectStrategy.MockedObject;
}
else if (mockStrategy is SubstituteBehaviorStrategy methodStrategy)
{
	var serviceSubstitute = Container.Resolve<ServiceGetTemplate>(methodStrategy.MethodMockStrategy);
    returnedValue = serviceSubstitute.Get();
}
```
- Delete it
```csharp
//it don't remove if strategy should always be applied
mockStrategyQuery.RemoveStrategy(mockStrategy);
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
