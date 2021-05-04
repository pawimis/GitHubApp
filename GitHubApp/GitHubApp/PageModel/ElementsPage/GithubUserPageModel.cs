using GitHubApp.Model;
using GitHubApp.PageModel.Base;

using MvvmCross.Commands;
using MvvmCross.Navigation;

using Octokit;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubApp.PageModel.ElementsPage
{

    public class GithubUserPageModel : BasePageModel<User>
    {
        #region Constructors
        public GithubUserPageModel(IMvxNavigationService navigationService) : base(navigationService)
        {
            NavigateToRepositoryCommand = new MvxAsyncCommand<Repository>(async (param) => await MvxNavigationService.Navigate<RepositoryPageModel, Repository>(param));

        }
        #endregion Constructors

        #region Events
        #endregion Events

        #region Fields
        private User _user;
        private IReadOnlyList<Repository> _userRepositories;
        private Dictionary<int, List<ContributionContainer>> contributions;
        #endregion Fields

        #region Services
        #endregion Services

        #region Properties
        public Dictionary<int, List<ContributionContainer>> Contributions
        {
            get => contributions;
            set => SetProperty(ref contributions, value);
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
        #endregion Properties

        #region Commands
        public IMvxAsyncCommand<Repository> NavigateToRepositoryCommand { get; }
        #endregion Commands

        #region Private Methods

        #endregion Private Methods

        #region Public Methods
        public override void Prepare(User parameter)
        {
            base.Prepare();
            User = parameter;
        }
        public override async Task Initialize()
        {
            await base.Initialize();
            await BusyManager.SetBusy();
            if (User != null)
            {
                User = await GitHubClientService.User.Get(User.Login);
                UserRepositories = await GitHubClientService.Repository.GetAllForUser(User.Login);
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
            await BusyManager.SetUnBusy();

        }
        #endregion PublicMethods
    }


}
