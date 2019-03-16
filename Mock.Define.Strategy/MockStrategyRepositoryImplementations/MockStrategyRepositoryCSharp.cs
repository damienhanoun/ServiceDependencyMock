using Mock.Dependency.With.Proxy.Data.Transfer.Objects.DatabaseEntities.CSharp;
using Mock.Dependency.With.Proxy.Data.Transfer.Objects.Strategies;
using MockStrategiesCSharp;
using System.Collections.Generic;
using System.Linq;

namespace Mock.Dependency.With.Proxy.Define.Strategy
{
    public class MockStrategyRepositoryCSharp : MockStrategyRepository
    {
        private readonly List<MockStrategy> savedMockedStrategy;

        public MockStrategyRepositoryCSharp()
        {
            this.savedMockedStrategy = new List<MockStrategy>();
        }

        public void DontMock(ForceNoMockStrategy noMockStrategy)
        {
            var row = new MockStrategyEntity
            {
                MethodId = noMockStrategy.MethodId,
                UniqueId = noMockStrategy.Id,
                SerializedStrategy = Serializer.Serialise(noMockStrategy)
            };
            MockStrategies.MockStrategy.Add(row);
            this.savedMockedStrategy.Add(noMockStrategy);
        }

        public void MockMethod(SubstituteBehaviorStrategy mockMethodStrategy)
        {
            var row = new MockStrategyEntity
            {
                MethodId = mockMethodStrategy.MethodId,
                UniqueId = mockMethodStrategy.Id,
                SerializedStrategy = Serializer.Serialise(mockMethodStrategy)
            };
            MockStrategies.MockStrategy.Add(row);
            this.savedMockedStrategy.Add(mockMethodStrategy);
        }

        public void MockObject<T>(ObjectStrategy<T> mockObjectStrategy)
        {
            var row = new MockStrategyEntity
            {
                MethodId = mockObjectStrategy.MethodId,
                UniqueId = mockObjectStrategy.Id,
                SerializedStrategy = Serializer.Serialise(mockObjectStrategy)
            };
            MockStrategies.MockStrategy.Add(row);
            this.savedMockedStrategy.Add(mockObjectStrategy);
        }

        public void RemoveStrategy(MockStrategy mockStrategy)
        {
            var row = MockStrategies.MockStrategy
                .First(m => m.UniqueId == mockStrategy.Id);
            MockStrategies.MockStrategy.Remove(row);
        }

        public void CleanUnUsedStrategiesDefinedByThisRepository()
        {
            foreach (var strategy in this.savedMockedStrategy)
                this.RemoveStrategy(strategy);
        }
    }
}
