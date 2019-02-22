namespace Business.Mock.ServiceMethods.Get
{
    public class ServiceGetSaved : ServiceGet
    {
        private readonly ServiceSideQuery serviceSideQuery;
        private readonly Service service;

        public ServiceGetSaved(ServiceSideQuery serviceSideQuery, Service service)
        {
            this.serviceSideQuery = serviceSideQuery;
            this.service = service;
        }

        public override int Get()
        {
            var context = this.serviceSideQuery.GetNextContext<GetContext>(ServiceIdentifier.GetIdentifier);
            if (context.SessionId == ApplicationDatabase.SessionId)
                return context.MockedObject;

            return this.service.Get();
        }
    }
}
