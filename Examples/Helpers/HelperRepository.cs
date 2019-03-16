using Mock.Dependency.With.Proxy.Data.Transfer.Objects.Strategies;
using System.Collections.Generic;

namespace IntegrationTests.Helpers
{
    interface HelperRepository
    {
        IEnumerable<MockStrategy> GetStrategies();
    }
}
