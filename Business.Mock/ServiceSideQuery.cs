using Optional;
using ServiceDependencyMock;

namespace Business.Mock
{
    public interface ServiceSideQuery
    {
        Option<MockType> GetNextMock(string methodIdentifier);
        T GetNextContext<T>(string methodIdentifier);
    }
}
