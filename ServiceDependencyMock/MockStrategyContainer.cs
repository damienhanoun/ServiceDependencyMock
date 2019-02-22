namespace ServiceDependencyMock
{
    public class MockStrategyContainer
    {
        public string MethodIdentifier;
        public string Strategy;
        public dynamic Context;
        public bool IsUsed;

        public bool IsMethodStrategy => this.Strategy != DefaultStrategies.ObjectOnly && this.Strategy != null;
        public bool IsObjectStrategy => this.Strategy == DefaultStrategies.ObjectOnly;
        public bool IsDefaultStrategy => this.Strategy == null;
    }
}
