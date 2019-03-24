namespace ExternalDependency
{
    public class ExternalServiceImpl : ExternalService
    {
        public int BrokenMethod()
        {
            throw new System.Exception("Something bad happen");
        }
    }
}
