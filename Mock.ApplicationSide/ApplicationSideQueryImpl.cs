using Mock.Library;
using Mock.Library.ApplicationSide;
using Optional;
using Optional.Unsafe;
using SharedDatabase;
using System.Linq;

namespace Mock.ApplicationSide
{
    public class ApplicationSideQueryImpl : ApplicationSideQuery
    {
        public Option<MockStrategy> GetMockStrategy(string methodIdentifier)
        {
            return MockStrategiesDatabase.MockStrategies
                        .Where(m => m.MethodId == methodIdentifier)
                        .Select(Option.Some)
                        .DefaultIfEmpty(Option.None<MockStrategy>())
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
