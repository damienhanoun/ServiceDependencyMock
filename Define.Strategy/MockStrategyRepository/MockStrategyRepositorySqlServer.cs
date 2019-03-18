using Microsoft.EntityFrameworkCore;
using Mock.Dependency.With.Proxy.Data.Transfer.Objects.DatabaseEntities.SqlServer;
using Mock.Dependency.With.Proxy.Data.Transfer.Objects.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MockStrategy = Mock.Dependency.With.Proxy.Data.Transfer.Objects.Strategies.MockStrategy;

namespace Mock.Dependency.With.Proxy.Define.Strategy
{
    public class MockStrategyRepositorySqlServer : MockStrategyRepository
    {
        private readonly DbContextOptionsBuilder<MockStrategiesContext> optionsBuilder;
        private List<MockStrategy> savedMockedStrategies { get; set; }

        public MockStrategyRepositorySqlServer(string connectionString)
        {
            this.optionsBuilder = new DbContextOptionsBuilder<MockStrategiesContext>();
            this.optionsBuilder.UseSqlServer(connectionString);
            this.savedMockedStrategies = new List<MockStrategy>();
        }

        public void DontMock(ForceNoMockStrategy noMockStrategy)
        {
            using (var context = new MockStrategiesContext(this.optionsBuilder.Options))
            {
                this.PreventBadCreationDateOrderingWhenThisOneEqualsToPreviousOne();

                var mockStrategySqlServer = noMockStrategy.ToSqlServerFormat();
                context.MockStrategy.Add(mockStrategySqlServer);
                context.SaveChanges();
            }

            this.savedMockedStrategies.Add(noMockStrategy);
        }

        public void MockBehavior(SubstituteBehaviorStrategy mockMethodStrategy)
        {
            using (var context = new MockStrategiesContext(this.optionsBuilder.Options))
            {
                this.PreventBadCreationDateOrderingWhenThisOneEqualsToPreviousOne();

                var mockStrategySqlServer = mockMethodStrategy.ToSqlServerFormat();
                context.MockStrategy.Add(mockStrategySqlServer);
                context.SaveChanges();
            }

            this.savedMockedStrategies.Add(mockMethodStrategy);
        }

        public void MockObject<T>(ObjectStrategy<T> mockObjectStrategy)
        {
            using (var context = new MockStrategiesContext(this.optionsBuilder.Options))
            {
                this.PreventBadCreationDateOrderingWhenThisOneEqualsToPreviousOne();

                var mockStrategySqlServer = mockObjectStrategy.ToSqlServerFormat();
                context.MockStrategy.Add(mockStrategySqlServer);
                context.SaveChanges();
            }

            this.savedMockedStrategies.Add(mockObjectStrategy);
        }

        private void PreventBadCreationDateOrderingWhenThisOneEqualsToPreviousOne()
        {
            Thread.Sleep(30);
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

        public void CleanUnUsedStrategiesDefinedByThisRepository()
        {
            foreach (var strategy in this.savedMockedStrategies)
            {
                try
                {
                    this.RemoveStrategy(strategy);
                }
                catch (InvalidOperationException) { }
            }

            this.savedMockedStrategies.RemoveAll(_ => true);
        }

        public List<MockStrategy> GetUnusedStrategiesCreatedByThisRepository()
        {
            var unusedStrategies = new List<MockStrategy>();

            using (var context = new MockStrategiesContext(this.optionsBuilder.Options))
            {
                foreach (var mockStrategy in this.savedMockedStrategies)
                {
                    var exist = context.MockStrategy.Any(m => m.Id == mockStrategy.Id);
                    if (exist)
                        unusedStrategies.Add(mockStrategy);
                }
            }

            return unusedStrategies;
        }
    }
}
