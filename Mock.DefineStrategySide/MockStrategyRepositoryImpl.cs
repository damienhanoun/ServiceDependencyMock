using Mock.Library;
using Mock.Library.DefineStrategySide;
using SharedDatabase;

namespace Mock.DefineStrategySide
{
    public class MockStrategyRepositoryImpl : MockRepository
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
