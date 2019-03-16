using Mock.Dependency.With.Proxy.Data.Transfer.Objects.Strategies;
using System;

namespace Mock.Dependency.With.Proxy.Apply.Strategy
{
    public interface MockStrategyRepository
    {
        MockStrategy GetMockStrategy(string methodIdentifier, Func<MockStrategy, bool> expression);
        void RemoveStrategy(MockStrategy mockStrategy);
    }
}
