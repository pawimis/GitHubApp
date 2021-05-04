using GitHubApp.Utils.BusyManager;

using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

using Octokit;

using System.Threading.Tasks;

using Xamarin.Forms;

namespace GitHubApp.PageModel.Base
{
    public abstract class BasePageModel<TParameter, TResult> : BasePageModel, IMvxViewModel<TParameter, TResult>
    {
        #region Constructors
        public BasePageModel(IMvxNavigationService mvxNavigationService) : base(mvxNavigationService) { }
        #endregion Constructors

        #region Properties
        public TaskCompletionSource<object> CloseCompletionSource { get; set; }
        #endregion Properties

        #region Public Methods
        public virtual void Prepare(TParameter parameter) { }
        public override void ViewDestroy(bool viewFinishing = true)
        {
            if (CloseCompletionSource != null && !CloseCompletionSource.Task.IsCompleted && !CloseCompletionSource.Task.IsFaulted)
            {
                CloseCompletionSource?.TrySetCanceled();
            }

            base.ViewDestroy(viewFinishing);
        }
        #endregion Public Methods
    }
    public abstract class BasePageModel<TParameter> : BasePageModel, IMvxViewModel<TParameter>
    {
        #region Constructors
        public BasePageModel(IMvxNavigationService mvxNavigationService) : base(mvxNavigationService) { }
        #endregion Constructors

        #region Public Methods
        public virtual void Prepare(TParameter parameter) { }
        #endregion Public Methods
    }
    public abstract class BasePageModel : MvxViewModel
    {
        #region Fields
        private bool _canNavigateBack;
        private string _title;
        private bool _showBack;
        #endregion Fields

        #region Constructors
        public BasePageModel(IMvxNavigationService mvxNavigationService) : base()
        {
            MvxNavigationService = mvxNavigationService;
            GitHubClientService = (GitHubClient)Mvx.IoCProvider.Resolve<IGitHubClient>();
            CloseCommand = new MvxAsyncCommand(ExecuteCloseCommand);
            BusyManager = Mvx.IoCProvider.Resolve<IBusyManager>();
            BusyManager.BusyChangedEvent += BusyChanged;

        }
        #endregion Constructors

        #region Properties
        protected GitHubClient GitHubClientService;
        protected IBusyManager BusyManager { get; }
        protected virtual IMvxNavigationService MvxNavigationService { get; }
        public virtual bool Busy => BusyManager.IsBusy;

        public virtual string Title
        {
            get => _title;
            set
            {
                _title = value;
                RaisePropertyChanged(nameof(Title));
            }
        }

        public virtual bool CanNavigateBack
        {
            get => _canNavigateBack || ShowBack;
            set
            {
                _canNavigateBack = value;
                RaisePropertyChanged(nameof(CanNavigateBack));
            }
        }
        protected bool ShowBack
        {
            get => _showBack;
            set
            {
                _showBack = value;
                RaisePropertyChanged(nameof(CanNavigateBack));
            }
        }
        #endregion Properties

        #region Commands

        public IMvxAsyncCommand CloseCommand { get; }

        #endregion Commands

        #region Protected Methods
        protected virtual bool IsAuthenticated()
        {
            return GitHubClientService != null && GitHubClientService.Credentials != null;
        }

        protected virtual Task ExecuteCloseCommand()
        {
            return MvxNavigationService.Close(this);
        }


        #endregion Protected Methods

        #region Private Methods
        private void BusyChanged(object sender, BusyChangedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                RaisePropertyChanged(nameof(Busy));
            });
        }

        #endregion Private Methods
    }
}
