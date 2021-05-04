using GitHubApp.Page.Base;
using GitHubApp.PageModel.Detail.Children;

using MvvmCross.Forms.Presenters.Attributes;

namespace GitHubApp.Page.Detail.Children
{
    [MvxTabbedPagePresentation(position: TabbedPosition.Tab, Title = "User", WrapInNavigationPage = false, Icon = "account")]
    public partial class UserPage : BaseContentPage<UserPageModel>
    {
        public UserPage()
        {
            InitializeComponent();

        }
    }
}