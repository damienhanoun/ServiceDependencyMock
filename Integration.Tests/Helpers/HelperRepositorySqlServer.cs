using Microsoft.EntityFrameworkCore;
using Mock.Dependency.With.Proxy.Apply.Strategy;
using Mock.Dependency.With.Proxy.Data.Transfer.Objects.DatabaseEntities.SqlServer;
using System.Collections.Generic;
using System.Linq;
using MockStrategy = Mock.Dependency.With.Proxy.Data.Transfer.Objects.Strategies.MockStrategy;

namespace Integration.Tests.Helpers
{
    class HelperRepositorySqlServer : HelperRepository
    {
        private readonly DbContextOptionsBuilder<MockStrategiesContext> optionsBuilder;

        public HelperRepositorySqlServer(DbContextOptionsBuilder<MockStrategiesContext> optionsBuilder)
        {
            this.optionsBuilder = optionsBuilder;
        }

        public IEnumerable<MockStrategy> GetStrategies()
        {
            using (var context = new MockStrategiesContext(this.optionsBuilder.Options))
            {
                return context.MockStrategy.ToList()
                        .DeserializeMockStrategies();
            }
        }

        public void RemoveAllStrategies()
        {
            using (var context = new MockStrategiesContext(this.optionsBuilder.Options))
            {
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE MockStrategy");
            }
        }
    }
}
