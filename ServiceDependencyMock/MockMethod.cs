namespace ServiceDependencyMock
{
    public class MockType
    {
        public string MethodIdentifier;
        public string Strategy;
        public dynamic Context;
        public bool IsUsed;

        public bool IsMethodStrategy => this.Strategy != Strategies.ObjectOnly && this.Strategy != null;
        public bool IsObjectStrategy => this.Strategy == Strategies.ObjectOnly;
        public bool IsDefaultStrategy => this.Strategy == null;
    }
}
