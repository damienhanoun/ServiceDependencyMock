using System;

namespace Mock.Library.ApplyStrategySide
{
    public interface MockStrategyQuery
    {
        MockStrategy GetMockStrategy(string methodIdentifier, Func<MockStrategy, bool> expression);
        T GetCurrentContext<T>(string methodIdentifier);
        void RemoveStrategy(MockStrategy mockStrategy);
    }
}
