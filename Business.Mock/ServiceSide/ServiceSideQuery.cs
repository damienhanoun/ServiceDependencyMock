using Optional;
using ServiceDependencyMock;

namespace Business.Mock.ServiceSide
{
    public interface ServiceSideQuery
    {
        Option<MockStrategyContainer> GetNextMock(string methodIdentifier);
        T GetNextContext<T>(string methodIdentifier);
    }
}
