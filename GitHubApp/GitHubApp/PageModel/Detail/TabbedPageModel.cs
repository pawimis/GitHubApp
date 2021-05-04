using GitHubApp.PageModel.Base;
using GitHubApp.PageModel.Detail.Children;

using MvvmCross.Navigation;

using Xamarin.Forms;

namespace GitHubApp.PageModel.Detail
{

    public class TabbedPageModel : BasePageModel
    {
        #region Constructors
        public TabbedPageModel(IMvxNavigationService mvxNavigationService) : base(mvxNavigationService)
        {
        }
        #endregion Constructors

        #region Events
        #endregion Events

        #region Fields
        private bool _firstTime = true;

        #endregion Fields

        #region Services
        #endregion Services

        #region Properties
        #endregion Properties

        #region Commands
        #endregion Commands

        #region Private Methods
        private void InitializeViewModels()
        {
            MvxNavigationService.Navigate<UserPageModel>();
            MvxNavigationService.Navigate<SearchPageModel>();

        }
        #endregion Private Methods

        #region Public Methods
        public override void ViewAppearing()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (_firstTime)
                {
                    InitializeViewModels();
                    _firstTime = false;
                }
            });
            base.ViewAppeared();
        }
        #endregion PublicMethods
    }



}
