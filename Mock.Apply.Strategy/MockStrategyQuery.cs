using Mock.Data.Tranfer.Objects.Strategies;
using System;

namespace Mock.Apply.Strategy
{
    public interface MockStrategyQuery
    {
        MockStrategy GetMockStrategy(string methodIdentifier, Func<MockStrategy, bool> expression);
        void RemoveStrategy(MockStrategy mockStrategy);
    }
}
