using System;

namespace DatabasesObjects.SqlServer
{
    public class MockStrategy
    {
        public string Id { get; set; }
        public string MethodId { get; set; }
        public byte[] SerializedStrategy { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
