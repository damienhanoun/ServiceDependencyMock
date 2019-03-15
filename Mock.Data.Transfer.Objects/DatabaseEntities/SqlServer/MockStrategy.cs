using System;

namespace Mock.Dependency.With.Proxy.Data.Transfer.Objects.DatabaseEntities.SqlServer
{
    public class MockStrategy
    {
        public string Id { get; set; }
        public string MethodId { get; set; }
        public byte[] SerializedStrategy { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
