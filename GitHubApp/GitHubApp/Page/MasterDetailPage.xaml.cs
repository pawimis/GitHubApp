using GitHubApp.PageModel;

using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;

namespace GitHubApp.Page
{
    [MvxMasterDetailPagePresentation(MasterDetailPosition.Root, WrapInNavigationPage = false)]
    public partial class MasterDetailPage : MvxMasterDetailPage<MasterDetailPageModel>
    {
        public MasterDetailPage()
        {
            InitializeComponent();
        }
    }
}