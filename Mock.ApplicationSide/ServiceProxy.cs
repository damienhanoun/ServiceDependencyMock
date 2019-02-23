using ExternalDependency;
using Mock.ApplicationSide.ServiceMethodsStrategies;
using Mock.ApplicationSide.ServiceMethodsStrategies.Get;
using Mock.Library;
using Mock.Library.ApplicationSide;
using System;
using Unity;
using static Mock.ApplicationSide.IOCContainer;

namespace Mock.ApplicationSide
{
    public class ServiceProxy : Service
    {
        private readonly Service service;
        private readonly ApplicationSideQuery serviceSideQuery;

        public ServiceProxy(ApplicationSideQuery serviceSideQuery, Service service)
        {
            this.serviceSideQuery = serviceSideQuery;
            this.service = service;
        }

        public int Get()
        {
            int returnedValue;

            var mockStrategy = this.serviceSideQuery.GetMockStrategy(ServiceMethodsIdentifiers.Get)
                                                    .ValueOr(new NoMockStrategy());

            if (NoMockStrategy(mockStrategy) || !InWantedContext(mockStrategy))
            {
                returnedValue = this.service.Get();
            }
            else if (mockStrategy is MethodToMockWithObjectStrategy<int> objectStrategy)
            {
                returnedValue = objectStrategy.MockedObject;
            }
            else if (mockStrategy is MethodToMockWithMethodStrategy methodStrategy)
            {
                returnedValue = ApplyMethodMockStrategy(methodStrategy);
            }
            else
            {
                throw new Exception("Current mock strategy is not take in account");
            }

            if (!(mockStrategy is NoMockStrategy))
                this.serviceSideQuery.RemoveStrategy(mockStrategy);

            return returnedValue;
        }

        private static bool NoMockStrategy(MockStrategy mockStrategy)
        {
            return mockStrategy is NoMockStrategy || mockStrategy is ForceNoMockStrategy;
        }

        private static bool InWantedContext(MockStrategy mockStrategy)
        {
            bool inWantedContext = true;
            mockStrategy.Context.MatchSome(c =>
            {
                var context = c as GetMockContext;
                if (context.SessionId != ApplicationDatabase.SessionId)
                    inWantedContext = false;
            });
            return inWantedContext;
        }

        private static int ApplyMethodMockStrategy(MethodToMockWithMethodStrategy methodStrategy)
        {
            try
            {
                var serviceSubstitute = Container.Resolve<ServiceGetTemplate>(methodStrategy.Strategy);
                return serviceSubstitute.Get();
            }
            catch (ResolutionFailedException)
            {
                throw new Exception($"Method strategy '{methodStrategy.Strategy}' is not defined");
            }
        }
    }
}
