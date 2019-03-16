using ExternalDependency;
using Mock.Dependency.With.Proxy.Apply.Strategy;
using Mock.Dependency.With.Proxy.Data.Transfer.Objects.Strategies;
using System;
using Unity;
using YourApplication.ServiceMethodsStrategies.Get;
using static YourApplication.IOCContainer;
using static YourApplication.ServiceMethodsStrategies.ServiceMethodsIdentifiers;

namespace YourApplication
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
            else if (mockStrategy is SubstituteBehaviorStrategy substituteStrategy)
            {
                returnedValue = ApplyMethodMockStrategy(substituteStrategy);
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

        private static int ApplyMethodMockStrategy(SubstituteBehaviorStrategy substituteStrategy)
        {
            try
            {
                var serviceSubstitute = Container.Resolve<ServiceGetTemplate>(substituteStrategy.BehaviorName);
                return serviceSubstitute.Get();
            }
            catch (ResolutionFailedException)
            {
                throw new Exception($"Method strategy '{substituteStrategy.BehaviorName}' is not defined");
            }
        }
    }
}
