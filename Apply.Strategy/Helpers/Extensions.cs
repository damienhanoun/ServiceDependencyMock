using Mock.Dependency.With.Proxy.Data.Transfer.Objects.Strategies;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Integration.Tests")]

namespace Mock.Dependency.With.Proxy.Apply.Strategy
{
    internal static class Extensions
    {
        public static List<MockStrategy> DeserializeMockStrategies(this IEnumerable<Data.Transfer.Objects.DatabaseEntities.SqlServer.MockStrategy> rows)
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
