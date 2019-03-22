using Mock.Dependency.With.Proxy.Data.Transfer.Objects.Strategies;
using System;

namespace Mock.Dependency.With.Proxy.Apply.Strategy
{
    public class ProxyBase
    {
        protected virtual Func<MockStrategy, bool> InWantedContext()
        {
            return s => { return true; };
        }
    }
}
