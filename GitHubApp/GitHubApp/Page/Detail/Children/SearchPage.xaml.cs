using GitHubApp.Page.Base;
using GitHubApp.PageModel.Detail.Children;

using MvvmCross.Forms.Presenters.Attributes;

namespace GitHubApp.Page.Detail.Children
{
    [MvxTabbedPagePresentation(position: TabbedPosition.Tab, Title = "Search", WrapInNavigationPage = false, Icon = "magnify")]
    public partial class SearchPage : BaseContentPage<SearchPageModel>
    {
        public SearchPage()
        {
            InitializeComponent();
        }


    }
}