using Mock.Dependency.With.Proxy.Define.Strategy;
using YourApplication;

namespace ProjectWhichDefineStrategies
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create strategy
            var mockMethodStrategy = MockStrategyBuilder.ForMethod("")
                .OnceWithMethodMockStrategy("strategy");

            //Store strategy
            var repository = new MockStrategyRepositorySqlServer("");
            repository.MockMethod(mockMethodStrategy);

            //Call your code which will use your defined strategy
            var service = new YourOwnService();
            service.MethodWhichNeedToCallExternalDependency();
        }
    }
}
