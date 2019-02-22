using ServiceDependencyMock;

namespace Business.Mock.ClientSide
{
    public class ClientSideRepositoryImpl : ClientSideRepository
    {
        public void Mock(MockStrategyContainer mockType)
        {
            FakeDatabase.MockTypes.Add(mockType);
        }
    }
}
