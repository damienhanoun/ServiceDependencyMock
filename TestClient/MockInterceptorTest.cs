using Business;
using Castle.DynamicProxy;
using NFluent;
using ServiceDependencyMock;
using Xunit;

namespace TestClient
{
    public class MockInterceptorTest
    {
        [Fact]
        public void Should_mock_one_method()
        {
            //Arrange
            var generator = new ProxyGenerator();
            var service = generator.CreateInterfaceProxyWithTarget<Service>(new ServiceImpl(), new MockInterceptor());

            //Act
            var value = service.Get();

            //Assert
            Check.That(value).IsEqualTo(2);
        }
    }
}
