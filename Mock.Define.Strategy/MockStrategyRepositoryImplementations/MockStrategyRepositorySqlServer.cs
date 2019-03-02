using DatabasesObjects.SqlServer;
using Mock.Strategies;
using System;

namespace Mock.Define.Strategy.MockStrategyRepositoryImplementations
{
    public class MockStrategyRepositorySqlServer : MockStrategyRepository
    {
        public void DontMock(ForceNoMockStrategy noMockStrategy)
        {
            using (var context = new MockStrategiesContext())
            {
                var mockStrategy = new DatabasesObjects.SqlServer.MockStrategy
                {
                    Id = noMockStrategy.Id,
                    MethodId = noMockStrategy.MethodId,
                    CreationDate = DateTime.UtcNow,
                    SerializedStrategy = Serializer.Serialise(noMockStrategy)
                };
                context.MockStrategy.Add(mockStrategy);
                context.SaveChanges();
            }
        }

        public void MockMethod(MethodToMockWithMethodStrategy mockMethodStrategy)
        {
            using (var context = new MockStrategiesContext())
            {
                var mockStrategy = new DatabasesObjects.SqlServer.MockStrategy
                {
                    Id = mockMethodStrategy.Id,
                    MethodId = mockMethodStrategy.MethodId,
                    CreationDate = DateTime.UtcNow,
                    SerializedStrategy = Serializer.Serialise(mockMethodStrategy)
                };
                context.MockStrategy.Add(mockStrategy);
                context.SaveChanges();
            }
        }

        public void MockObject<T>(MethodToMockWithObjectStrategy<T> mockObjectStrategy)
        {
            using (var context = new MockStrategiesContext())
            {
                var mockStrategy = new DatabasesObjects.SqlServer.MockStrategy
                {
                    Id = mockObjectStrategy.Id,
                    MethodId = mockObjectStrategy.MethodId,
                    CreationDate = DateTime.UtcNow,
                    SerializedStrategy = Serializer.Serialise(mockObjectStrategy)
                };
                context.MockStrategy.Add(mockStrategy);
                context.SaveChanges();
            }
        }
    }
}
