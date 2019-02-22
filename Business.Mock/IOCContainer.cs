using Business.Mock.ClientSide;
using Business.Mock.ServiceSide;
using Business.Mock.ServiceSide.ServiceMethodsStrategies.Get;
using Unity;

namespace Business.Mock
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
                    container.RegisterType<ClientSideRepository, ClientSideRepositoryImpl>();
                    container.RegisterType<ServiceSideQuery, ServiceSideQueryImpl>();
                    container.RegisterType<ServiceGetTemplate, ServiceGetOne>(nameof(ServiceGetOne));
                    container.RegisterType<ServiceGetTemplate, ServiceGetGeneratedObject>(nameof(ServiceGetGeneratedObject));
                }

                return container;
            }
        }
    }
}
