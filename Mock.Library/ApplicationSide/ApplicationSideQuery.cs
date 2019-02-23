using Optional;

namespace Mock.Library.ApplicationSide
{
    public interface ApplicationSideQuery
    {
        Option<MockStrategy> GetMockStrategy(string methodIdentifier);
        T GetCurrentContext<T>(string methodIdentifier);
        void RemoveStrategy(MockStrategy mockStrategy);
    }
}
