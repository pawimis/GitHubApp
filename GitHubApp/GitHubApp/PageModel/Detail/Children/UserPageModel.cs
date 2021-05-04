using GitHubApp.Interface;
using GitHubApp.Model;
using GitHubApp.PageModel.Base;
using GitHubApp.PageModel.ElementsPage;
using GitHubApp.Utils;

using MvvmCross.Commands;
using MvvmCross.Navigation;

using Octokit;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;

namespace GitHubApp.PageModel.Detail.Children
{

    public class UserPageModel : BasePageModel
    {
        #region Constructors
        public UserPageModel(IMvxNavigationService mvxNavigationService, IPopupNavigationService popupNavigationServce, ISecureStorageService secureStorageService) : base(mvxNavigationService)
        {
            LoginCommand = new MvxAsyncCommand(LoginUser);
            NavigateToRepositoryCommand = new MvxAsyncCommand<Repository>(async (param) => await MvxNavigationService.Navigate<RepositoryPageModel, Repository>(param));
            _popupNavigationServce = popupNavigationServce;
            _secureStorageService = secureStorageService;
            GenerateToken = new MvxAsyncCommand(GetTokenFromWebpage);
            Token = "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";
        }
        #endregion Constructors

        #region Events
        #endregion Events

        #region Fields
        private string _Token;
        private bool _tokenEntryNotValid;
        private bool _notAuthenticated;
        private LayoutState _currentState = LayoutState.Empty;
        private bool _rememberMe;

        private User _user;
        private IReadOnlyList<Repository> _userRepositories;
        private Dictionary<int, List<ContributionContainer>> contributions;

        #endregion Fields

        #region Services
        private readonly IPopupNavigationService _popupNavigationServce;
        private readonly ISecureStorageService _secureStorageService;
        #endregion Services

        #region Properties
        public Dictionary<int, List<ContributionContainer>> Contributions
        {
            get => contributions;
            set => SetProperty(ref contributions, value);
        }

        public bool RememberMe
        {
            get => _rememberMe;
            set => SetProperty(ref _rememberMe, value);
        }

        public User User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public IReadOnlyList<Repository> UserRepositories
        {
            get => _userRepositories;
            set => SetProperty(ref _userRepositories, value);
        }

        public bool TokenEntryNotValid
        {
            get => _tokenEntryNotValid;
            set => SetProperty(ref _tokenEntryNotValid, value);
        }

        public LayoutState CurrentState
        {
            get => _currentState;
            set => SetProperty(ref _currentState, value);
        }

        public bool NotAuthenticated
        {
            get => _notAuthenticated;
            set => SetProperty(ref _notAuthenticated, value);
        }

        public string Token
        {
            get => _Token;
            set => SetProperty(ref _Token, value);
        }

        #endregion Properties

        #region Commands
        public IMvxAsyncCommand LoginCommand { get; }
        public IMvxAsyncCommand GenerateToken { get; }
        public IMvxAsyncCommand<Repository> NavigateToRepositoryCommand { get; }
        #endregion Commands

        #region Private Methods
        private async Task GetTokenFromWebpage()
        {
            try
            {
                await Browser.OpenAsync("https://github.com/settings/tokens", BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                // An unexpected error occured. No browser may be installed on the device.
            }
        }
        private async Task LoginUser()
        {
            CurrentState = LayoutState.Loading;
            if (ValidateInput())
            {
                GitHubClientService.Credentials = new Credentials(Token);
                if (RememberMe)
                {
                    await _secureStorageService.Set(StorageKeys.USER_TOKEN, Token);
                }
                else
                {
                    await _secureStorageService.Set(StorageKeys.USER_TOKEN, string.Empty);
                }
                User = await GitHubClientService.User.Current();

                if (User == null)
                {
                    await _popupNavigationServce.PushPopup(false, string.Empty, "Incorrect username or password");
                    CurrentState = LayoutState.Empty;
                }
                else
                {
                    await GetUserData();
                    CurrentState = LayoutState.Success;
                }
            }
            else
            {
                CurrentState = LayoutState.Empty;
            }
        }

        private async Task GetUserData()
        {
            UserRepositories = await GitHubClientService.Repository.GetAllForCurrent();
            List<ContributionContainer> list = new List<ContributionContainer>();
            foreach (Repository item in UserRepositories)
            {
                if (item.CreatedAt == item.UpdatedAt)
                {
                    list.Add(new ContributionContainer { ContributionDate = item.CreatedAt.Date, ContributionType = Model.Enums.ContributionTypeEnum.Created, RepositoryName = item.Name });
                }
                else
                {
                    list.Add(new ContributionContainer { ContributionDate = item.CreatedAt.Date, ContributionType = Model.Enums.ContributionTypeEnum.Created, RepositoryName = item.Name });
                    list.Add(new ContributionContainer { ContributionDate = item.UpdatedAt.Date, ContributionType = Model.Enums.ContributionTypeEnum.Updated, RepositoryName = item.Name });
                }
            }
            Contributions = list.GroupBy(x => x.ContributionDate.Year)
                .OrderBy(x => x.Key)
                .ToDictionary(g => g.Key, g => g.ToList());


        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(Token) || (Token.Length < 4 || Token.Length > 100))
            {
                TokenEntryNotValid = true;
                return false;
            }
            return true;
        }
        #endregion Private Methods

        #region Public Methods
        public override async Task Initialize()
        {
            await base.Initialize();
            Token = await _secureStorageService.Get(StorageKeys.USER_TOKEN);
            RememberMe = !string.IsNullOrWhiteSpace(Token);
            if (RememberMe)
            {
                LoginUser();
            }
            NotAuthenticated = !IsAuthenticated();
        }
        #endregion PublicMethods
    }


}
