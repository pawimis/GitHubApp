using System.Threading.Tasks;

namespace GitHubApp.Interface
{
    public interface ISecureStorageService
    {
        Task Set(string key, string password);
        Task<string> Get(string key);
        void RemoveAll();

    }
}
