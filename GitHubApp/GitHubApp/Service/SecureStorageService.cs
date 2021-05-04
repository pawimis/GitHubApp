using GitHubApp.Interface;
using MonkeyCache.FileStore;
using System;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace GitHubApp.Service
{
    public class SecureStorageService : ISecureStorageService
    {

        public async Task<string> Get(string key)
        {
            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    return await SecureStorage.GetAsync(key);
                case Device.iOS:
                    return Barrel.Current.Get<string>(key);
                default:
                    return null;
            }
        }

        public void RemoveAll()
        {
            SecureStorage.RemoveAll();
        }

        public async Task Set(string key, string password)
        {
            try
            {
                switch (Device.RuntimePlatform)
                {
                    case Device.Android:
                        await SecureStorage.SetAsync(key, password);
                        break;
                    case Device.iOS:
                        Barrel.Current.Add<string>(key, password, new TimeSpan(100, 0, 0));
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new NotSupportedException(message: "Device does not support secure storage. " + ex.ToString());
            }
        }
    }
}
