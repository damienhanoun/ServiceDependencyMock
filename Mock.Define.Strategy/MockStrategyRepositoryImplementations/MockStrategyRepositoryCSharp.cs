using Mock.Dependency.With.Proxy.Data.Transfer.Objects.DatabaseEntities.CSharp;
using Mock.Dependency.With.Proxy.Data.Transfer.Objects.Strategies;
using MockStrategiesCSharp;
using System.Linq;

namespace Mock.Dependency.With.Proxy.Define.Strategy
{
    public class MockStrategyRepositoryCSharp : MockStrategyRepository
    {
        public void DontMock(ForceNoMockStrategy noMockStrategy)
        {
            var row = new MockStrategyEntity
            {
                MethodId = noMockStrategy.MethodId,
                UniqueId = noMockStrategy.Id,
                SerializedStrategy = Serializer.Serialise(noMockStrategy)
            };
            MockStrategies.MockStrategy.Add(row);
        }

        public void MockMethod(MethodToMockWithMethodStrategy mockMethodStrategy)
        {
            var row = new MockStrategyEntity
            {
                MethodId = mockMethodStrategy.MethodId,
                UniqueId = mockMethodStrategy.Id,
                SerializedStrategy = Serializer.Serialise(mockMethodStrategy)
            };
            MockStrategies.MockStrategy.Add(row);
        }

        public void MockObject<T>(MethodToMockWithObjectStrategy<T> mockObjectStrategy)
        {
            var row = new MockStrategyEntity
            {
                MethodId = mockObjectStrategy.MethodId,
                UniqueId = mockObjectStrategy.Id,
                SerializedStrategy = Serializer.Serialise(mockObjectStrategy)
            };
            MockStrategies.MockStrategy.Add(row);
        }

        public void RemoveStrategy(MockStrategy mockStrategy)
        {
            var row = MockStrategies.MockStrategy
                .First(m => m.UniqueId == mockStrategy.Id);
            MockStrategies.MockStrategy.Remove(row);
        }
    }
}
