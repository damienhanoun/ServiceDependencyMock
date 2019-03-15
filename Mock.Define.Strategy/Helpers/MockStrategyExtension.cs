using Mock.Dependency.With.Proxy.Data.Transfer.Objects.Strategies;
using System;
using MockStrategySqlServer = Mock.Dependency.With.Proxy.Data.Transfer.Objects.DatabaseEntities.SqlServer.MockStrategy;

namespace Mock.Dependency.With.Proxy.Define.Strategy
{
    internal static class MockStrategyExtension
    {
        public static MockStrategySqlServer ToSqlServerFormat(this MockStrategy mockStrategy)
        {
            return new MockStrategySqlServer
            {
                Id = mockStrategy.Id,
                MethodId = mockStrategy.MethodId,
                CreationDate = DateTime.UtcNow,
                SerializedStrategy = Serializer.Serialise(mockStrategy)
            };
        }
    }
}
