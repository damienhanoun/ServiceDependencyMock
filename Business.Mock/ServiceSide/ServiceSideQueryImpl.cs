using Optional;
using ServiceDependencyMock;
using System.Linq;

namespace Business.Mock.ServiceSide
{
    public class ServiceSideQueryImpl : ServiceSideQuery
    {
        public Option<MockStrategyContainer> GetCurrentMock(string methodIdentifier)
        {
            var mockOption = FakeDatabase.MockTypes
                        .Where(m => m.MethodIdentifier == methodIdentifier)
                        .Where(m => m.IsUsed == false)
                        .Select(Option.Some)
                        .DefaultIfEmpty(Option.None<MockStrategyContainer>())
                        .First();

            mockOption.MatchSome(mock => mock.IsUsed = true);

            return mockOption;
        }

        public T GetCurrentContext<T>(string methodIdentifier)
        {
            return (T)FakeDatabase.MockTypes
                        .Where(m => m.MethodIdentifier == methodIdentifier)
                        .First(m => m.IsUsed == false)
                        .Context;
        }
    }
}
