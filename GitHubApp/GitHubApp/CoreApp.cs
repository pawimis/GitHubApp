using GitHubApp.PageModel;

using MvvmCross.ViewModels;

namespace GitHubApp
{
    public class CoreApp : MvxApplication
    {
        public override void Initialize()
        {
            base.Initialize();

            RegisterStartPage();
        }

        private void RegisterStartPage()
        {
            RegisterAppStart<MasterDetailPageModel>();
        }
    }
}
