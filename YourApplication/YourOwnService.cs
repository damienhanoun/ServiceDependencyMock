using ExternalDependency;
using Unity;

namespace YourApplication
{
    public class YourOwnService
    {
        public void MethodWhichNeedToCallExternalDependency()
        {
            //Get the service proxy
            var externalService = IOCContainer.Container.Resolve<ExternalService>();
            var result = externalService.BrokenMethod();
            //Do something with the result
        }
    }
}
