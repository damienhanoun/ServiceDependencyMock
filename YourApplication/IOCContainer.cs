using ExternalDependency;
using Microsoft.Extensions.Configuration;
using Mock.Dependency.With.Proxy.Apply.Strategy;
using Unity;
using Unity.Injection;

namespace YourApplication
{
    public static class IOCContainer
    {
        internal static IUnityContainer container;

        public static IUnityContainer Container
        {
            get
            {
                if (container == null)
                {
                    container = new UnityContainer();

                    var connectionString = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json")
                        .Build()["ConnectionString"].ToString();

                    container.RegisterType<ExternalService>(new InjectionFactory(c =>
                    {
                        return new ExternalServiceProxy(
                            new MockStrategyRepositorySqlServer(connectionString),
                            new ExternalServiceImpl());
                    }));
                }

                return container;
            }
        }
    }
}
