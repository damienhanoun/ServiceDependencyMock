using Microsoft.EntityFrameworkCore;
using Mock.Apply.Strategy.Helpers;
using Mock.Data.Tranfer.Objects.DatabaseEntities.SqlServer;
using System;
using System.Linq;
using MockStrategy = Mock.Data.Tranfer.Objects.Strategies.MockStrategy;

namespace Mock.Apply.Strategy.MockStrategyQueryImplementations
{
    public class MockStrategyQuerySqlServer : MockStrategyQuery
    {
        private readonly DbContextOptionsBuilder<MockStrategiesContext> optionsBuilder;

        public MockStrategyQuerySqlServer(string connectionString)
        {
            this.optionsBuilder = new DbContextOptionsBuilder<MockStrategiesContext>();
            this.optionsBuilder.UseSqlServer(connectionString);
        }

        public MockStrategy GetMockStrategy(string methodIdentifier, Func<MockStrategy, bool> inWantedContext)
        {
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
            using (var context = new MockStrategiesContext(this.optionsBuilder.Options))
            {
                var dbMockStrategy = context.MockStrategy.First(m => m.Id == mockStrategy.Id);
                context.MockStrategy.Remove(dbMockStrategy);
                context.SaveChanges();
            }
        }
    }
}
