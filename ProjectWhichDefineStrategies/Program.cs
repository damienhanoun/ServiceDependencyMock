using Mock.Dependency.With.Proxy.Define.Strategy;
using YourApplication;

namespace ProjectWhichDefineStrategies
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create strategy
            var mockStrategy = MockStrategyBuilder.ForMethod("")
                .OnceWithSubstituteBehavior("strategy");

            //Store strategy
            var repository = new MockStrategyRepositorySqlServer("");
            repository.MockMethod(mockStrategy);

            //Call your code which will use your defined strategy
            var service = new YourOwnService();
            service.MethodWhichNeedToCallExternalDependency();
        }
    }
}
