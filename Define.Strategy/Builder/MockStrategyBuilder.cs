namespace Mock.Dependency.With.Proxy.Define.Strategy
{
    public class MockStrategyBuilder
    {
        public static MethodToMock ForMethod(string methodId)
        {
            return new MethodToMock() { MethodId = methodId };
        }
    }
}
