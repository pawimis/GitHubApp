using GitHubApp.PageModel.Base;
using GitHubApp.PageModel.Detail;
using GitHubApp.PageModel.Master;

using MvvmCross.Navigation;
using MvvmCross.ViewModels;

using System.Threading.Tasks;

namespace GitHubApp.PageModel
{

    public class MasterDetailPageModel : BasePageModel
    {
        #region Constructors
        public MasterDetailPageModel(IMvxNavigationService mvxNavigationService) : base(mvxNavigationService)
        {
        }
        #endregion Constructors

        #region Events
        #endregion Events

        #region Fields
        #endregion Fields

        #region Services
        #endregion Services

        #region Properties
        #endregion Properties

        #region Commands
        #endregion Commands

        #region Private Methods
        private async Task InitializeViewModels()
        {
            await MvxNavigationService.Navigate<MenuPageModel>();
            await MvxNavigationService.Navigate<TabbedPageModel>();

        }
        #endregion Private Methods

        #region Public Methods
        public override void ViewAppeared()
        {
            base.ViewAppeared();
            MvxNotifyTask.Create(async () => await InitializeViewModels());
        }
        #endregion PublicMethods
    }
}
