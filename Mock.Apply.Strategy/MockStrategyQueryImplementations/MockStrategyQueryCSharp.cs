using Mock.Dependency.With.Proxy.Data.Transfer.Objects.Strategies;
using MockStrategiesCSharp;
using System;
using System.Linq;

namespace Mock.Dependency.With.Proxy.Apply.Strategy
{
    public class MockStrategyQueryCSharp : MockStrategyQuery
    {
        public MockStrategy GetMockStrategy(string methodIdentifier, Func<MockStrategy, bool> inWantedContext)
        {
            return MockStrategies.MockStrategy
                        .Where(m => m.MethodId == methodIdentifier)
                        .DeserializeMockStrategies()
                        .Where(inWantedContext)
                        .DefaultIfEmpty(new NoMockStrategy())
                        .First();
        }

        public void RemoveStrategy(MockStrategy mockStrategy)
        {
            if (mockStrategy is NoMockStrategy || mockStrategy.IsAlwaysApplied)
                return;

            var row = MockStrategies.MockStrategy
                    .First(m => m.UniqueId == mockStrategy.Id);
            MockStrategies.MockStrategy.Remove(row);
        }
    }
}
