using Optional;
using ServiceDependencyMock;

namespace Business.Mock.ServiceSide
{
    public interface ServiceSideQuery
    {
        Option<MockStrategyContainer> GetCurrentMock(string methodIdentifier);
        T GetCurrentContext<T>(string methodIdentifier);
    }
}
