using Business.Mock.ServiceSide;
using Business.Mock.ServiceSide.ServiceMethodsStrategies;
using Business.Mock.ServiceSide.ServiceMethodsStrategies.Get;
using System;
using Unity;
using static Business.Mock.IOCContainer;

namespace Business.Mock
{
    public class ServiceProxy : Service
    {
        private readonly Service service;
        private readonly ServiceSideQuery serviceSideQuery;

        public ServiceProxy(ServiceSideQuery serviceSideQuery, Service service)
        {
            this.serviceSideQuery = serviceSideQuery;
            this.service = service;
        }

        public int Get()
        {
            int? returnedValue = null;

            var mockTypeOption = this.serviceSideQuery.GetCurrentMock(ServiceMethodsIdentifiers.GetId);

            mockTypeOption.MatchSome(mockType =>
            {
                if (mockType.IsObjectStrategy)
                {
                    returnedValue = ((MockContext)mockType.Context).MockedObject;
                }
                else if (mockType.IsMethodStrategy)
                {
                    returnedValue = ApplyMethodMockStrategy(mockType);
                }
                else if (mockType.IsDefaultStrategy)
                {
                    returnedValue = this.service.Get();
                }
                else
                {
                    throw new Exception("Current mockType can't be used");
                }
            });

            mockTypeOption.MatchNone(() => returnedValue = this.service.Get());

            return returnedValue.Value;
        }

        private static int? ApplyMethodMockStrategy(ServiceDependencyMock.MockStrategyContainer mockType)
        {
            int? returnedValue;

            try
            {
                var serviceSubstitute = Container.Resolve<ServiceGetTemplate>(mockType.Strategy);
                returnedValue = serviceSubstitute.Get();
            }
            catch (ResolutionFailedException)
            {
                throw new Exception($"Method strategy '{mockType.Strategy}' is not defined");
            }

            return returnedValue;
        }
    }
}
