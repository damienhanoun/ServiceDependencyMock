using Mock.Dependency.With.Proxy.Define.Strategy;
using System.Configuration;
using YourApplication;
using static CentralizedInformations.ExtenalServiceMethodsIds;

namespace ProjectWhichDefineStrategies
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create strategy
            var mockStrategy = MockStrategyBuilder.ForMethod(BrokenMethodId)
                .OnceWithSubstituteBehavior("ServiceGetOne");

            //Store strategy
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            var repository = new MockStrategyRepositorySqlServer(connectionString);
            repository.MockBehavior(mockStrategy);

            //Those lines should not be here
            //Just imagine it is a javascript call to a .NET Core Web site project
            var service = new YourOwnService();
            service.MethodWhichNeedToCallExternalDependency();
        }
    }
}
