using GitHubApp.Interface;

using System;
using System.Threading.Tasks;

using Xamarin.Essentials;

namespace GitHubApp.Service
{
    public class SecureStorageService : ISecureStorageService
    {
        public async Task<string> Get(string key)
        {
            return await SecureStorage.GetAsync(key);
        }

        public void RemoveAll()
        {
            SecureStorage.RemoveAll();
        }

        public async Task Set(string key, string password)
        {
            try
            {
                await SecureStorage.SetAsync(key, password);
            }
            catch (Exception ex)
            {
                throw new NotSupportedException(message: "Device does not support secure storage. " + ex.ToString());
            }
        }
    }
}
