using GitHubApp.Page.Base;
using GitHubApp.PageModel.ElementsPage;

using MvvmCross.Forms.Presenters.Attributes;

namespace GitHubApp.Page.ElementsPage
{
    [MvxContentPagePresentation(Animated = true, NoHistory = false)]
    public partial class RepositoryPage : BaseContentPage<RepositoryPageModel>
    {
        public RepositoryPage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, true);
        }
    }
}