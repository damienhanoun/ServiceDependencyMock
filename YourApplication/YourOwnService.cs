using ExternalDependency;
using Unity;

namespace YourApplication
{
    public class YourOwnService
    {
        public void MethodWhichNeedToCallExternalDependency()
        {
            var externalService = IOCContainer.Container.Resolve<ExternalService>();
            var result = externalService.Get();
            //Do something with the result
        }
    }
}
