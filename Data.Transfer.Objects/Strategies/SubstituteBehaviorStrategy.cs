using System;

namespace Mock.Dependency.With.Proxy.Data.Transfer.Objects.Strategies
{
    [Serializable]
    public class SubstituteBehaviorStrategy : MockStrategy
    {
        public string BehaviorName;

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var mockStrategy = (SubstituteBehaviorStrategy)obj;
            return this.BehaviorName == mockStrategy.BehaviorName
                && base.Equals(obj);
        }
    }
}
