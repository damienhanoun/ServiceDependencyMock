using Microsoft.EntityFrameworkCore;
using Mock.Dependency.With.Proxy.Data.Transfer.Objects.DatabaseEntities.SqlServer;
using Mock.Dependency.With.Proxy.Data.Transfer.Objects.Strategies;
using System.Linq;
using System.Threading;
using MockStrategy = Mock.Dependency.With.Proxy.Data.Transfer.Objects.Strategies.MockStrategy;

namespace Mock.Dependency.With.Proxy.Define.Strategy
{
    public class MockStrategyRepositorySqlServer : MockStrategyRepository
    {
        private readonly DbContextOptionsBuilder<MockStrategiesContext> optionsBuilder;

        public MockStrategyRepositorySqlServer(string connectionString)
        {
            this.optionsBuilder = new DbContextOptionsBuilder<MockStrategiesContext>();
            this.optionsBuilder.UseSqlServer(connectionString);
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
        }

        public void MockMethod(MethodToMockWithMethodStrategy mockMethodStrategy)
        {
            using (var context = new MockStrategiesContext(this.optionsBuilder.Options))
            {
                this.PreventBadCreationDateOrderingWhenThisOneEqualsToPreviousOne();

                var mockStrategySqlServer = mockMethodStrategy.ToSqlServerFormat();
                context.MockStrategy.Add(mockStrategySqlServer);
                context.SaveChanges();
            }
        }

        public void MockObject<T>(MethodToMockWithObjectStrategy<T> mockObjectStrategy)
        {
            using (var context = new MockStrategiesContext(this.optionsBuilder.Options))
            {
                this.PreventBadCreationDateOrderingWhenThisOneEqualsToPreviousOne();

                var mockStrategySqlServer = mockObjectStrategy.ToSqlServerFormat();
                context.MockStrategy.Add(mockStrategySqlServer);
                context.SaveChanges();
            }
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
    }
}
