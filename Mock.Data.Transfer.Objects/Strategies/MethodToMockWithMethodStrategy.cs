using System;

namespace Mock.Data.Tranfer.Objects.Strategies
{
    [Serializable]
    public class MethodToMockWithMethodStrategy : MockStrategy
    {
        public string MethodMockStrategy;

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var mockStrategy = (MethodToMockWithMethodStrategy)obj;
            return this.MethodMockStrategy == mockStrategy.MethodMockStrategy
                && base.Equals(obj);
        }
    }
}
