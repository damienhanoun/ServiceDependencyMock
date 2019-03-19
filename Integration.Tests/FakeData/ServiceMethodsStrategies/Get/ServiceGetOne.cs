namespace Integration.Tests.ProjectWithProxy.ServiceMethodsStrategies.Get
{
    public class ServiceGetOne : ExternalServiceGetTemplate
    {
        public int Get()
        {
            return 1;
        }
    }
}
