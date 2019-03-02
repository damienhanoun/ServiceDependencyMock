using Mock.Apply.Strategy.Helpers;
using Mock.Strategies;
using System;
using System.Linq;
using MockStrategiesContext = DatabasesObjects.SqlServer.MockStrategiesContext;

namespace Mock.Apply.Strategy.MockStrategyQueryImplementations
{
    public class MockStrategyQuerySqlServer : MockStrategyQuery
    {
        public MockStrategy GetMockStrategy(string methodIdentifier, Func<MockStrategy, bool> inWantedContext)
        {
            using (var context = new MockStrategiesContext())
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
            using (var context = new MockStrategiesContext())
            {
                var dbMockStrategy = context.MockStrategy.First(m => m.Id == mockStrategy.Id);
                context.MockStrategy.Remove(dbMockStrategy);
                context.SaveChanges();
            }
        }
    }
}
