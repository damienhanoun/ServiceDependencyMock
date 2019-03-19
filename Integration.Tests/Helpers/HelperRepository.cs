using Mock.Dependency.With.Proxy.Data.Transfer.Objects.Strategies;
using System.Collections.Generic;

namespace Integration.Tests.Helpers
{
    interface HelperRepository
    {
        IEnumerable<MockStrategy> GetStrategies();

        void RemoveAllStrategies();
    }
}
