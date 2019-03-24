using Mock.Dependency.With.Proxy.Apply.Strategy;

namespace YourApplication.ServiceMethodsStrategies.Get
{
    public class ServiceGetOne : ExternalServiceBrokenMethodTemplate
    {
        public int BrokenMethod()
        {
            return 1;
        }
    }
}
