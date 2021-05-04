using GitHubApp.Page.Base;
using GitHubApp.PageModel.Master;

using MvvmCross.Forms.Presenters.Attributes;

namespace GitHubApp.Page.Master
{
    [MvxMasterDetailPagePresentation(MasterDetailPosition.Master, Title = "GitHub")]
    public partial class MenuPage : BaseContentPage<MenuPageModel>
    {
        public MenuPage()
        {
            InitializeComponent();
        }
    }
}