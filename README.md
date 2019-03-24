# Intent
The project goal is to allow to mock calls to external services in your application by defining a set of reusable patterns and creating libraries for common behavior 

This will allow to deal with the following cases :
- launch end to end test in a mastered environment
- mock a service when it is broken and could take time to be up again (in all environment but production)

# Global solution overview
The solution consist in three parts :
- Creating a proxy in front of the wanted service which will be able to apply a mock strategy (return a created object, define a different behavior or just call the real service)
- Since proxy need to know how to behave, a database is needed to get the strategy to apply in the proxy
- Since the database need to be configured, another project is needed to define the wanted strategy to apply in it

Those modules are represented in the "Libraries/Sources" directory

# Important note
If there is multiple time the same name for a method in a class, it will not work (for now)

# Installation
- Install database with sql server project (Libraries/Sources/Centralized/MockStrategiesSqlServer)
- In the project which will set up the mock strategy in the database, get [Mock.Dependency.With.Proxy.Define.Strategy](https://www.nuget.org/packages/Mock.Dependency.With.Proxy.Define.Strategy) nuget package
- In your main project, get [Mock.Dependency.With.Proxy.Apply.Strategy](https://www.nuget.org/packages/Mock.Dependency.With.Proxy.Apply.Strategy) nuget package

# Minimal Configuration

## In a shared project
### Create ids for each proxyfied methods 
Default name convention : methodName + "Id"

You can create a shared project to set ids in it like :
```csharp
public struct ExternalServiceMethodsIdentifiers
{
    public static readonly string MethodId = "6228e927-eceb-4a7a-a179-7e4f1bc17d85";
}
```

## In your project 
### Generate proxies
In the project where you will apply the strategy, a GenerateProxy.tt file will be added to your project (Doesn't work for .NET Core project for now)

Copy past it for each assembly in which you want to create proxy

In each tt file, follow the "TODO" instructions to be able to generate all proxies

The default convention name for proxy is : className + "Proxy"

### Update your IOC container with proxy informations
```csharp
container.RegisterType<ExternalService>(new InjectionFactory(c =>
{
    return new ExternalServiceProxy(
        new MockStrategyRepositorySqlServer(connectionString),
        new ExternalServiceInitialImpl(/*c.Resolve<...>(), ...*/));
}));
```

# How to use it

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

## Apply the mock strategy through a proxy

### Apply it only in a specific context
```csharp
public partial class ExternalServiceProxy
{
    private YourContextGetter yourContextGetter;

    public ExternalServiceProxy(MockStrategyRepository mockStrategyQuery, ExternalService service,
        YourContextGetter yourContextGetter) : base(mockStrategyQuery, service)
    {
        this.yourContextGetter = yourContextGetter;
    }

    protected override Func<MockStrategy, bool> InWantedContext()
    {
        return strategy =>
        {
            bool inWantedContext = true;
            strategy.Context.MatchSome(c =>
            {
                var context = c as GetMockContext;
                var sessionId = this.yourContextGetter.GetYourSessionId();
                if (context.SessionId != null && context.SessionId != sessionId)
                    inWantedContext = false;
            });
            return inWantedContext;
        };
    }
}
```

### Create special behavior for a method
Just create a class which extend his correspond template :
```csharp
public class ExternalServiceGetOne : ExternalClassNameMehtodTemplate
{
    public int Mehtod() { return 1; }
}
```

### For end to end test purpose, clean defined mock
At the end of each test, call :
```csharp
this.defineMockStrategyRepository.CleanUnUsedStrategiesDefinedByThisRepository();
```
to be sure others tests will not be impacted by some unused strategies


# Todo list

## Functionalities
- Make it work with multiple methods with the same name in a class
- Find an elegant way to get the context in a class used as strategy (to define a specific behavior)
- Create a "mock only on error" strategy
- Add some analyse method to be sure that all call has been used and not more or less for end to end test purpose
- Not have to use method id. Look for NSubstitute like behavior with DynamicProxy like "myService.Get(Arg.Any()).Return(...)""
- Create a view to allow mock on demand

## Examples
- With context that help calculation in a mocked method
- With parameter base strategy
- With multiple methods for the main class
- With builder for object strategy
