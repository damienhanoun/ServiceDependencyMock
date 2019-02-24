namespace Mock.Library.DefineStrategySide
{
    public class MethodToMock
    {
        public string MethodId;

        public MethodToMockWithMethodStrategy WithStrategy(string strategy)
        {
            return new MethodToMockWithMethodStrategy
            {
                MethodId = MethodId,
                Strategy = strategy
            };
        }

        public MethodToMockWithObjectStrategy<T> WithObject<T>(T mockedObject)
        {
            return new MethodToMockWithObjectStrategy<T>
            {
                MethodId = MethodId,
                MockedObject = mockedObject
            };
        }

        public ForceNoMockStrategy WithoutMock()
        {
            return new ForceNoMockStrategy
            {
                MethodId = MethodId
            };
        }
    }
}
