using System.Threading.Tasks;

namespace ExternalDependency
{
    public interface ExternalService
    {
        int Get();
        void Set(int i);
        Task<int> GetAsync();
        void SetAsync(int i);
    }
}
