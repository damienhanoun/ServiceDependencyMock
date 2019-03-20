using System.Threading.Tasks;

namespace ExternalDependency
{
    public class ExternalServiceImpl : ExternalService
    {
        public int Get()
        {
            return 0;
        }

        public void Set(int i)
        {
            throw new System.NotImplementedException();
        }

        public async Task<int> GetAsync()
        {
            return await Task.FromResult(0);
        }

        public async void SetAsync(int i)
        {
            throw new System.NotImplementedException();
        }
    }
}
