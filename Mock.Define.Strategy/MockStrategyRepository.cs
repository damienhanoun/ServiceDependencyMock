using Mock.Strategies;

namespace Mock.Define.Strategy
{
    public interface MockStrategyRepository
    {
        void MockMethod(MethodToMockWithMethodStrategy mockMethodStrategy);
        void MockObject<T>(MethodToMockWithObjectStrategy<T> mockObjectStrategy);
        void DontMock(ForceNoMockStrategy noMockStrategy);
    }
}
