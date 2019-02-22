using Business.Mock.ServiceMethods;
using Business.Mock.ServiceMethods.Get;
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

            var mockTypeOption = this.serviceSideQuery.GetNextMock(ServiceIdentifier.GetIdentifier);

            mockTypeOption.MatchSome(mockType =>
            {
                if (mockType.IsObjectStrategy)
                {
                    returnedValue = ((GetContext)mockType.Context).MockedObject;
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

        private static int? ApplyMethodMockStrategy(ServiceDependencyMock.MockType mockType)
        {
            int? returnedValue;

            try
            {
                var serviceSubstitute = Container.Resolve<ServiceGet>(mockType.Strategy);
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
