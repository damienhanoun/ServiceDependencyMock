using Optional;
using System;

namespace Mock.Dependency.With.Proxy.Data.Transfer.Objects.Strategies
{
    [Serializable]
    public class ForceNoMockStrategy : MockStrategy
    {
        public ForceNoMockStrategy WithContext(dynamic context)
        {
            this.Context = Option.Some<dynamic>(context);
            return this;
        }
    }
}
