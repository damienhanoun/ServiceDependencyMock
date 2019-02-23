namespace Mock.Library.ClientSide
{
    public interface ClientSideRepository
    {
        void MockMethod(MethodToMockWithMethodStrategy mockObjectStrategy);
        void MockObject<T>(MethodToMockWithObjectStrategy<T> mockObjectStrategy);
        void DontMock(ForceNoMockStrategy noMockStrategy);
    }
}
