using Optional;
using Optional.Unsafe;
using System;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("Apply.Strategy")]
[assembly: InternalsVisibleTo("Define.Strategy")]

namespace Mock.Dependency.With.Proxy.Data.Transfer.Objects.Strategies
{
    [Serializable]
    public class MockStrategy
    {
        internal string Id = Guid.NewGuid().ToString();
        public string MethodId;
        public bool IsAlwaysApplied;
        public Option<dynamic> Context;

        public MockStrategy()
        {
            this.IsAlwaysApplied = false;
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
                (!mockStrategy.Context.HasValue && !this.Context.HasValue)
                && mockStrategy.IsAlwaysApplied == this.IsAlwaysApplied;
        }
    }
}
