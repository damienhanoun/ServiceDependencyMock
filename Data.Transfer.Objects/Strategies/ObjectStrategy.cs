using System;

namespace Mock.Dependency.With.Proxy.Data.Transfer.Objects.Strategies
{
    [Serializable]
    public class ObjectStrategy<T> : MockStrategy
    {
        public T MockedObject;

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var mockStrategy = (ObjectStrategy<T>)obj;
            return this.MockedObject.Equals(mockStrategy.MockedObject)
                && base.Equals(obj);
        }
    }
}
