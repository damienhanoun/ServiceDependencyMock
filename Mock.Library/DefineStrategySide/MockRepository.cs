namespace Mock.Library.DefineStrategySide
{
    public interface MockRepository
    {
        void MockMethod(MethodToMockWithMethodStrategy mockObjectStrategy);
        void MockObject<T>(MethodToMockWithObjectStrategy<T> mockObjectStrategy);
        void DontMock(ForceNoMockStrategy noMockStrategy);
    }
}
