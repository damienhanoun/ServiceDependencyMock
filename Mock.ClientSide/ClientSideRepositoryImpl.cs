using Mock.Library;
using Mock.Library.ClientSide;
using SharedDatabase;

namespace Mock.ClientSide
{
    public class ClientSideRepositoryImpl : ClientSideRepository
    {
        public void MockMethod(MethodToMockWithMethodStrategy mockObjectStrategy)
        {
            MockStrategiesDatabase.MockStrategies.Add(mockObjectStrategy);
        }

        public void MockObject<T>(MethodToMockWithObjectStrategy<T> mockObjectStrategy)
        {
            MockStrategiesDatabase.MockStrategies.Add(mockObjectStrategy);
        }

        public void DontMock(ForceNoMockStrategy noMockStrategy)
        {
            MockStrategiesDatabase.MockStrategies.Add(noMockStrategy);
        }
    }
}
