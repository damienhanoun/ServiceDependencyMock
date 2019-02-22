using Business.Mock.ServiceMethods.Get;
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
                    container.RegisterType<ServiceGet, ServiceGetOne>(nameof(ServiceGetOne));
                    container.RegisterType<ServiceGet, ServiceGetSaved>(nameof(ServiceGetSaved));
                }

                return container;
            }
        }
    }
}
