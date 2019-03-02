using System;

namespace Mock.Strategies
{
    [Serializable]
    public class MethodToMockWithObjectStrategy<T> : MockStrategy
    {
        public T MockedObject;

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var mockStrategy = (MethodToMockWithObjectStrategy<T>)obj;
            return this.MockedObject.Equals(mockStrategy.MockedObject)
                && base.Equals(obj);
        }
    }
}
