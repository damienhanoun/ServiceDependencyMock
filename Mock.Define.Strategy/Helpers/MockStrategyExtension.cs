using Mock.Strategies;
using System;

namespace Mock.Define.Strategy.Helpers
{
    public static class MockStrategyExtension
    {
        public static DatabasesObjects.SqlServer.MockStrategy ToSqlServerFormat(this MockStrategy mockStrategy)
        {
            return new DatabasesObjects.SqlServer.MockStrategy
            {
                Id = mockStrategy.Id,
                MethodId = mockStrategy.MethodId,
                CreationDate = DateTime.UtcNow,
                SerializedStrategy = Serializer.Serialise(mockStrategy)
            };
        }
    }
}
