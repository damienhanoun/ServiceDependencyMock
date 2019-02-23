using Optional;
using Optional.Unsafe;

namespace Mock.Library
{
    public class MockStrategy
    {
        public string MethodId;
        public Option<dynamic> Context;

        public MockStrategy()
        {
            this.Context = Option.None<dynamic>();
        }

        public override int GetHashCode()
        {
            return 89240;
        }

        public override bool Equals(object obj)
        {
            var mockStrategy = (MockStrategy)obj;
            return mockStrategy.MethodId == this.MethodId &&
                (mockStrategy.Context.HasValue && this.Context.HasValue &&
                    mockStrategy.Context.ValueOrFailure()
                    .Equals(this.Context.ValueOrFailure())) ||
                (!mockStrategy.Context.HasValue && !this.Context.HasValue);
        }
    }
}
