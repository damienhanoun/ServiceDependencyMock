using ServiceDependencyMock;

namespace Business.Mock
{
    public class ClientSideRepositoryImpl : ClientSideRepository
    {
        public void Mock(MockType mockType)
        {
            FakeDatabase.MockTypes.Add(mockType);
        }
    }
}
