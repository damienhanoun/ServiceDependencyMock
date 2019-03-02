using Mock.Apply.Strategy.Helpers;
using Mock.Strategies;
using MockStrategiesCSharp;
using System;
using System.Linq;

namespace Mock.Apply.Strategy.MockStrategyQueryImplementations
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
            var row = MockStrategies.MockStrategy
                        .First(m => m.UniqueId == mockStrategy.Id);
            MockStrategies.MockStrategy.Remove(row);
        }
    }
}
