namespace Mock.Library.ClientSide
{
    public class MockStrategyBuilder
    {
        public static MethodToMock ForMethod(string methodId)
        {
            return new MethodToMock() { MethodId = methodId };
        }
    }
}
