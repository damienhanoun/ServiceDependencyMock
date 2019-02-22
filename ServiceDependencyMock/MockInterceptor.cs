using Castle.DynamicProxy;

namespace ServiceDependencyMock
{
    public class MockInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var mockEnable = true;
            if (mockEnable)
                invocation.ReturnValue = 2;
            else
                invocation.Proceed();
        }
    }
}
