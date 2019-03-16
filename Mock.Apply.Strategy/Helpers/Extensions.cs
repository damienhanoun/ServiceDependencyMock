using Mock.Dependency.With.Proxy.Data.Transfer.Objects.DatabaseEntities.CSharp;
using Mock.Dependency.With.Proxy.Data.Transfer.Objects.Strategies;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("IntegrationTests")]

namespace Mock.Dependency.With.Proxy.Apply.Strategy
{
    internal static class Extensions
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

        public static List<MockStrategy> DeserializeMockStrategies(this IEnumerable<Mock.Dependency.With.Proxy.Data.Transfer.Objects.DatabaseEntities.SqlServer.MockStrategy> rows)
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
