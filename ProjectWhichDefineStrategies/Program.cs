using YourApplication;

namespace ProjectWhichDefineStrategies
{
    class Program
    {
        static void Main(string[] args)
        {
            //Configure database mock with some strategies

            //Call your code which will use your defined strategy
            var service = new YourOwnService();
            service.MethodWhichNeedToCallExternalDependency();
        }
    }
}
