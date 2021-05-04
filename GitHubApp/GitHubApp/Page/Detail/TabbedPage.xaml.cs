using GitHubApp.PageModel.Detail;

using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;

using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace GitHubApp.Page.Detail
{
    [MvxMasterDetailPagePresentation(MasterDetailPosition.Detail, NoHistory = true)]
    public partial class TabbedPage : MvxTabbedPage<TabbedPageModel>
    {
        public TabbedPage()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
            MvxNavigationPage.SetHasNavigationBar(this, false);
            Xamarin.Forms.PlatformConfiguration.AndroidSpecific.TabbedPage.SetIsSwipePagingEnabled(this, false);
        }
    }
}