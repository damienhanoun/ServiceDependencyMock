using Mock.Dependency.With.Proxy.Data.Transfer.Objects.Strategies;

namespace Mock.Dependency.With.Proxy.Define.Strategy
{
    public interface MockStrategyRepository
    {
        void MockMethod(MethodToMockWithMethodStrategy mockMethodStrategy);
        void MockObject<T>(MethodToMockWithObjectStrategy<T> mockObjectStrategy);
        void DontMock(ForceNoMockStrategy noMockStrategy);
        void RemoveStrategy(MockStrategy mockStrategy);
    }
}
