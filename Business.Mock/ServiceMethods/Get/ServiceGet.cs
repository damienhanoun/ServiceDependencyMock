using ServiceDependencyMock;

namespace Business.Mock.ServiceMethods.Get
{
    public abstract class ServiceGet : MethodMockStrategy
    {
        public string MethodIdentifier => ServiceIdentifier.GetIdentifier;

        public abstract int Get();
    }
}
