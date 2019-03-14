using Optional;
using System;

namespace Mock.Data.Tranfer.Objects.Strategies
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
