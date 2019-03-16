# Intent
The project goal is to allow to mock calls to external services in your application by defining a set of reusable patterns and creating libraries for common behavior 

This will allow to deal with the following cases :
- launch end to end test in a mastered environment
- mock a service when it is broken and could take time to be up again (in all environment but production)
- mock a service on all environment but production (for long call time or money reason)

# Installation
- Install database with sql project
- Look at DefaultCredentials.sql to be able to create connectionString according to it
- In the project which will set up the mock strategy in the database, get [Mock.Dependency.With.Proxy.Define.Strategy](https://www.nuget.org/packages/Mock.Dependency.With.Proxy.Define.Strategy) nuget package
- In your main project, get [Mock.Dependency.With.Proxy.Apply.Strategy](https://www.nuget.org/packages/Mock.Dependency.With.Proxy.Apply.Strategy) nuget package

# How it work
## Define a mock strategy
```csharp
using Mock.Dependency.With.Proxy.Define.Strategy;

//Object
var mockStrategy = MockStrategyBuilder.ForMethod("method id")
	.OnceWithObject(yourObject);

//Behavior
var mockStrategy = MockStrategyBuilder.ForMethod("method id")
	.OnceWithSubstituteBehavior("behavior name")

//Without anything (you may want to not mock between two mocks)
var mockStrategy = MockStrategyBuilder.ForMethod("method id")
    .OnceWithoutMock();

//Eventually with a context to apply only when needed or specify data to help to generate return value for substitute behavior
var mockStrategy = MockStrategyBuilder.ForMethod("method id").OnceWithoutMock();
	.WithContext(new Context(){ ... })

//Store it in a database
var repository = new MockStrategyRepositorySqlServer(connectionString);
repository.MockMethod(mockMethodStrategy);
```

## Apply the mock through a proxy
```csharp
using Mock.Dependency.With.Proxy.Apply.Strategy;
using Mock.Dependency.With.Proxy.Data.Transfer.Objects.Strategies;

//Get data from the database with filter or not
//If you want to be allow to desactivate mock, implement MockConfiguration and give it as parameter
var mockStrategyRepository = new MockStrategyRepository(connectionString /*, mockConfiguration */);
var mockStrategy = mockStrategyRepository.GetMockStrategy("method id", s =>
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

//Apply strategy
//NoMockStrategy is the default if there is no defined strategy or if library is desactivated
if (mockStrategy is NoMockStrategy || mockStrategy is ForceNoMockStrategy)
{
    returnedValue = service.Get();
}
else if (mockStrategy is ObjectStrategy<int> objectStrategy)
{
    returnedValue = objectStrategy.MockedObject;
}
else if (mockStrategy is SubstituteBehaviorStrategy substituteBehaviorStrategy)
{
	var serviceSubstitute = Container.Resolve<ServiceGetTemplate>(substituteBehaviorStrategy.BehaviorName);
    returnedValue = serviceSubstitute.Get();
}

//Delete strategy (not removed if strategy should always be applied)
mockStrategyRepository.RemoveStrategy(mockStrategy);
```

# Todo list

## Functionalities
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
