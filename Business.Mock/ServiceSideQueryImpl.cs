using Optional;
using ServiceDependencyMock;
using System.Linq;

namespace Business.Mock
{
    public class ServiceSideQueryImpl : ServiceSideQuery
    {
        public Option<MockType> GetNextMock(string methodIdentifier)
        {
            var mockOption = FakeDatabase.MockTypes
                        .Where(m => m.MethodIdentifier == methodIdentifier)
                        .Where(m => m.IsUsed == false)
                        .Select(m => Option.Some(m))
                        .DefaultIfEmpty(Option.None<MockType>())
                        .First();

            mockOption.MatchSome(mock => mock.IsUsed = true);

            return mockOption;
        }

        public T GetNextContext<T>(string methodIdentifier)
        {
            return (T)FakeDatabase.MockTypes
                        .Where(m => m.MethodIdentifier == methodIdentifier)
                        .Where(m => m.IsUsed == false)
                        .First().Context;
        }
    }
}
