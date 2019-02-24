using Mock.Library;
using Mock.Library.ApplyStrategySide;
using Optional.Unsafe;
using SharedDatabase;
using System;
using System.Linq;

namespace Mock.ApplyStrategySide
{
    public class MockStrategyQueryImpl : MockStrategyQuery
    {
        public MockStrategy GetMockStrategy(string methodIdentifier, Func<MockStrategy, bool> inWantedContext)
        {
            return MockStrategiesDatabase.MockStrategies
                        .Where(m => m.MethodId == methodIdentifier)
                        .Where(inWantedContext)
                        .DefaultIfEmpty(new NoMockStrategy())
                        .First();
        }

        public T GetCurrentContext<T>(string methodIdentifier)
        {
            return (T)MockStrategiesDatabase.MockStrategies
                        .First(m => m.MethodId == methodIdentifier)
                        .Context.ValueOrFailure();
        }

        public void RemoveStrategy(MockStrategy mockStrategy)
        {
            MockStrategiesDatabase.MockStrategies.Remove(mockStrategy);
        }
    }
}
