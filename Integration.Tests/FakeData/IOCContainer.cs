using Integration.Tests.ProjectWithProxy.ServiceMethodsStrategies.Get;
using Unity;

namespace Integration.Tests.ProjectWithProxy
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
                    container.RegisterType<ExternalServiceGetTemplate, ServiceGetOne>(nameof(ServiceGetOne));
                }

                return container;
            }
        }
    }
}
