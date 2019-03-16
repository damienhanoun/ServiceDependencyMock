using IntegrationTests.ExternalProject;
using IntegrationTests.ProjectWithProxy.ServiceMethodsStrategies.Get;
using Mock.Dependency.With.Proxy.Apply.Strategy;
using Mock.Dependency.With.Proxy.Data.Transfer.Objects.Strategies;
using System;
using Unity;
using static IntegrationTests.ProjectWithProxy.IOCContainer;
using static IntegrationTests.ProjectWithProxy.ServiceMethodsStrategies.ServiceMethodsIdentifiers;

namespace IntegrationTests.ProjectWithProxy
{
    public class ExternalServiceProxy : ExternalService
    {
        private readonly ExternalService service;
        private readonly MockStrategyRepository mockStrategyQuery;

        public ExternalServiceProxy(MockStrategyRepository mockStrategyQuery, ExternalService service)
        {
            this.mockStrategyQuery = mockStrategyQuery;
            this.service = service;
        }

        public int Get()
        {
            int returnedValue;

            var mockStrategy = this.mockStrategyQuery.GetMockStrategy(GetId, InWantedContext());

            if (NoMockStrategy(mockStrategy))
            {
                returnedValue = this.service.Get();
            }
            else if (mockStrategy is ObjectStrategy<int> objectStrategy)
            {
                returnedValue = objectStrategy.MockedObject;
            }
            else if (mockStrategy is SubstituteBehaviorStrategy methodStrategy)
            {
                returnedValue = ApplyMethodMockStrategy(methodStrategy);
            }
            else
            {
                throw new Exception("Current mock strategy is not take in account");
            }

            this.mockStrategyQuery.RemoveStrategy(mockStrategy);

            return returnedValue;
        }

        private static bool NoMockStrategy(MockStrategy mockStrategy)
        {
            return mockStrategy is NoMockStrategy || mockStrategy is ForceNoMockStrategy;
        }

        private static Func<MockStrategy, bool> InWantedContext()
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

        private static int ApplyMethodMockStrategy(SubstituteBehaviorStrategy substituteBehaviorStrategy)
        {
            try
            {
                var serviceSubstitute = Container.Resolve<ServiceGetTemplate>(substituteBehaviorStrategy.BehaviorName);
                return serviceSubstitute.Get();
            }
            catch (ResolutionFailedException)
            {
                throw new Exception($"Method strategy '{substituteBehaviorStrategy.BehaviorName}' is not defined");
            }
        }
    }
}
