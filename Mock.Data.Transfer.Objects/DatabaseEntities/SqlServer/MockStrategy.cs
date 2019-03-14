using System;

namespace Mock.Data.Tranfer.Objects.DatabaseEntities.SqlServer
{
    public class MockStrategy
    {
        public string Id { get; set; }
        public string MethodId { get; set; }
        public byte[] SerializedStrategy { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
