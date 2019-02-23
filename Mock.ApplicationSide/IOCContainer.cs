using Mock.ApplicationSide.ServiceMethodsStrategies.Get;
using Unity;

namespace Mock.ApplicationSide
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
                    container.RegisterType<ServiceGetTemplate, ServiceGetOne>(nameof(ServiceGetOne));
                }

                return container;
            }
        }
    }
}
