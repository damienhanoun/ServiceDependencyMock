using Mock.Data.Tranfer.Objects.Strategies;
using System;

namespace Mock.Define.Strategy.Helpers
{
    public static class MockStrategyExtension
    {
        public static Data.Tranfer.Objects.DatabaseEntities.SqlServer.MockStrategy ToSqlServerFormat(this MockStrategy mockStrategy)
        {
            return new Data.Tranfer.Objects.DatabaseEntities.SqlServer.MockStrategy
            {
                Id = mockStrategy.Id,
                MethodId = mockStrategy.MethodId,
                CreationDate = DateTime.UtcNow,
                SerializedStrategy = Serializer.Serialise(mockStrategy)
            };
        }
    }
}
