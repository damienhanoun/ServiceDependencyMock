using Integration.Tests.ProjectWithProxy.ServiceMethodsStrategies.Get;
using Mock.Dependency.With.Proxy.Data.Transfer.Objects.Strategies;
using System;

namespace Integration.Tests.ProjectWithProxy
{
    public partial class ExternalServiceProxy
    {
        protected override Func<MockStrategy, bool> InWantedContext()
        {
            return s =>
            {
                bool inWantedContext = true;
                s.Context.MatchSome(c =>
                {
                    var context = c as GetMockContext;
                    if (context.SessionId != null && context.SessionId != ApplicationDatabase.SessionId)
                        inWantedContext = false;
                });
                return inWantedContext;
            };
        }
    }
}
