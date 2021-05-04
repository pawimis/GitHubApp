using MonkeyCache.FileStore;

using System.Globalization;

using Xamarin.Forms;

namespace GitHubApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            CultureInfo culture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            Barrel.ApplicationId = "Canal_plus";

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
