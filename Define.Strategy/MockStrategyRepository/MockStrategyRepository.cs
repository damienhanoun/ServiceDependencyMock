using Mock.Dependency.With.Proxy.Data.Transfer.Objects.Strategies;

namespace Mock.Dependency.With.Proxy.Define.Strategy
{
    public interface MockStrategyRepository
    {
        void MockBehavior(SubstituteBehaviorStrategy mockMethodStrategy);
        void MockObject<T>(ObjectStrategy<T> mockObjectStrategy);
        void DontMock(ForceNoMockStrategy noMockStrategy);
        void RemoveStrategy(MockStrategy mockStrategy);
        void CleanUnUsedStrategiesDefinedByThisRepository();
    }
}
