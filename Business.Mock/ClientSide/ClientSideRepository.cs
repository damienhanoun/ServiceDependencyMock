using ServiceDependencyMock;

namespace Business.Mock.ClientSide
{
    public interface ClientSideRepository
    {
        void Mock(MockStrategyContainer mockType);
    }
}
