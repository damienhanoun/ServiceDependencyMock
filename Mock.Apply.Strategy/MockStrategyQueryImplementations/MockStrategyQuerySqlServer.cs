using Microsoft.EntityFrameworkCore;
using Mock.Dependency.With.Proxy.Data.Transfer.Objects.DatabaseEntities.SqlServer;
using System;
using System.Linq;
using MockStrategy = Mock.Dependency.With.Proxy.Data.Transfer.Objects.Strategies.MockStrategy;

namespace Mock.Dependency.With.Proxy.Apply.Strategy
{
    public class MockStrategyQuerySqlServer : MockStrategyQuery
    {
        private readonly DbContextOptionsBuilder<MockStrategiesContext> optionsBuilder;
        private readonly MockConfiguration mockConfiguration;

        public MockStrategyQuerySqlServer(string connectionString)
            : this(connectionString, new DefaultMockConfiguration())
        { }

        public MockStrategyQuerySqlServer(string connectionString, MockConfiguration mockConfiguration)
        {
            this.optionsBuilder = new DbContextOptionsBuilder<MockStrategiesContext>();
            this.optionsBuilder.UseSqlServer(connectionString);
            this.mockConfiguration = mockConfiguration;
        }

        public MockStrategy GetMockStrategy(string methodIdentifier, Func<MockStrategy, bool> inWantedContext)
        {
            if (!this.mockConfiguration.IsActivated())
                return new NoMockStrategy();

            using (var context = new MockStrategiesContext(this.optionsBuilder.Options))
            {
                return context.MockStrategy.Where(m => m.MethodId == methodIdentifier)
                        .OrderBy(m => m.CreationDate)
                        .ToList()
                        .DeserializeMockStrategies()
                        .Where(inWantedContext)
                        .DefaultIfEmpty(new NoMockStrategy())
                        .First();
            }
        }

        public void RemoveStrategy(MockStrategy mockStrategy)
        {
            if (mockStrategy is NoMockStrategy || mockStrategy.IsAlwaysApplied)
                return;

            using (var context = new MockStrategiesContext(this.optionsBuilder.Options))
            {
                var dbMockStrategy = context.MockStrategy.First(m => m.Id == mockStrategy.Id);
                context.MockStrategy.Remove(dbMockStrategy);
                context.SaveChanges();
            }
        }
    }
}
