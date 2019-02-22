namespace Business.Mock.ServiceSide.ServiceMethodsStrategies.Get
{
    public class ServiceGetGeneratedObject : ServiceGetTemplate
    {
        private readonly ServiceSideQuery serviceSideQuery;
        private readonly Service service;

        public ServiceGetGeneratedObject(ServiceSideQuery serviceSideQuery, Service service)
        {
            this.serviceSideQuery = serviceSideQuery;
            this.service = service;
        }

        public override int Get()
        {
            var context = this.serviceSideQuery.GetNextContext<MockContext>(ServiceMethodsIdentifiers.GetId);
            if (context.SessionId == ApplicationDatabase.SessionId)
                return context.MockedObject;

            return this.service.Get();
        }
    }
}
