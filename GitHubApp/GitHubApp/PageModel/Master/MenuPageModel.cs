using GitHubApp.Interface;
using GitHubApp.PageModel.Base;

using MvvmCross.Commands;
using MvvmCross.Navigation;

using System.Linq;
using System.Threading.Tasks;

namespace GitHubApp.PageModel.Master
{

    public class MenuPageModel : BasePageModel
    {
        #region Constructors
        public MenuPageModel(IMvxNavigationService mvxNavigationService, IGithubService githubService) : base(mvxNavigationService)
        {
            SendCommand = new MvxAsyncCommand(Send);
            _webService = githubService;

        }


        #endregion Constructors

        #region Events
        #endregion Events

        #region Fields
        private string _even;
        private string _ad;
        private string _numberText;
        #endregion Fields

        #region Services
        private IGithubService _webService { get; }

        #endregion Services

        #region Properties

        public string NumberText
        {
            get => _numberText;
            set => SetProperty(ref _numberText, value);
        }




        public string Even
        {
            get => _even;
            set => SetProperty(ref _even, value);
        }


        public string Ad
        {
            get => _ad;
            set => SetProperty(ref _ad, value);
        }

        #endregion Properties

        #region Commands
        public IMvxAsyncCommand SendCommand { get; }

        #endregion Commands

        #region Private Methods
        private async Task Send()
        {
            await BusyManager.SetBusy();
            if (!string.IsNullOrWhiteSpace(NumberText) && NumberText.All(char.IsDigit))
            {
                Service.Web.ServiceStatusMessage<Model.IsEven> result = await _webService.CheckIfIsEven(NumberText);
                if (result.DidSucceed && result.HasEntity)
                {
                    Even = result.Entity.Iseven.ToString();
                    Ad = result.Entity.Ad;
                }
            }
            await BusyManager.SetUnBusy();

        }
        #endregion Private Methods

        #region Public Methods
        #endregion PublicMethods
    }



}
