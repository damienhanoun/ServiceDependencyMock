using Mock.Data.Tranfer.Objects.DatabaseEntities.CSharp;
using Mock.Data.Tranfer.Objects.Strategies;
using System.Collections.Generic;

namespace Mock.Apply.Strategy.Helpers
{
    public static class Extensions
    {
        public static List<MockStrategy> DeserializeMockStrategies(this IEnumerable<MockStrategyEntity> rows)
        {
            var strategies = new List<MockStrategy>();

            foreach (var strategy in rows)
            {
                var mockStrategy = DeSerializer.DeSerialise<MockStrategy>(strategy.SerializedStrategy);
                strategies.Add(mockStrategy);
            }

            return strategies;
        }

        public static List<MockStrategy> DeserializeMockStrategies(this IEnumerable<Mock.Data.Tranfer.Objects.DatabaseEntities.SqlServer.MockStrategy> rows)
        {
            var strategies = new List<MockStrategy>();

            foreach (var strategy in rows)
            {
                var mockStrategy = DeSerializer.DeSerialise<MockStrategy>(strategy.SerializedStrategy);
                strategies.Add(mockStrategy);
            }

            return strategies;
        }
    }
}
