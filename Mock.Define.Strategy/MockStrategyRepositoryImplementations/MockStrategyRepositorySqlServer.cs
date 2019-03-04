using DatabasesObjects.SqlServer;
using Mock.Define.Strategy.Helpers;
using Mock.Strategies;
using System.Threading;

namespace Mock.Define.Strategy.MockStrategyRepositoryImplementations
{
    public class MockStrategyRepositorySqlServer : MockStrategyRepository
    {
        public void DontMock(ForceNoMockStrategy noMockStrategy)
        {
            using (var context = new MockStrategiesContext())
            {
                this.PreventBadCreationDateOrderingWhenThisOneEqualsToPreviousOne();

                var mockStrategySqlServer = noMockStrategy.ToSqlServerFormat();
                context.MockStrategy.Add(mockStrategySqlServer);
                context.SaveChanges();
            }
        }

        public void MockMethod(MethodToMockWithMethodStrategy mockMethodStrategy)
        {
            using (var context = new MockStrategiesContext())
            {
                this.PreventBadCreationDateOrderingWhenThisOneEqualsToPreviousOne();

                var mockStrategySqlServer = mockMethodStrategy.ToSqlServerFormat();
                context.MockStrategy.Add(mockStrategySqlServer);
                context.SaveChanges();
            }
        }

        public void MockObject<T>(MethodToMockWithObjectStrategy<T> mockObjectStrategy)
        {
            using (var context = new MockStrategiesContext())
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
    }
}
