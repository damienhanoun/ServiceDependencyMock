using System;

namespace Mock.Strategies
{
    [Serializable]
    public class MethodToMockWithMethodStrategy : MockStrategy
    {
        public string Strategy;

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var mockStrategy = (MethodToMockWithMethodStrategy)obj;
            return this.Strategy == mockStrategy.Strategy
                && base.Equals(obj);
        }
    }
}
