using GitHubApp.Interface;
using GitHubApp.Model;
using GitHubApp.Model.Enums;
using GitHubApp.PageModel.Base;
using GitHubApp.PageModel.ElementsPage;
using GitHubApp.Utils;

using MvvmCross.Commands;
using MvvmCross.Navigation;

using Octokit;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace GitHubApp.PageModel.Detail.Children
{

    public class SearchPageModel : BasePageModel
    {
        #region Constructors
        public SearchPageModel(IMvxNavigationService mvxNavigationService, ICacheService cacheService, IPopupNavigationService popupNavigationServce) : base(mvxNavigationService)
        {
            _cacheService = cacheService;
            EmptyViewText = "Tap on search icon to find what you desire";
            _popupNavigationServce = popupNavigationServce;

            SearchCommand = new MvxCommand(() =>
            {
                SearchHintList = _cacheService.GetCached<List<string>>(StorageKeys.SEARCH_CACHE);
                CurrentState = LayoutState.Success;
                HasHinsVisible = true;
            });
            CancelSearchCommand = new MvxCommand(() =>
            {
                CurrentState = LayoutState.Empty;
                HasHinsVisible = false;
            });
            SearchEnteredCommand = new MvxAsyncCommand(SearchGithub);
            SelectedResultCommand = new MvxAsyncCommand(ShowResult);
            FilterCommand = new MvxAsyncCommand<SearchTypeEnum>( async (param) => await ChangeDisplayedList(param));
            LoadMoreResultsCommand = new MvxAsyncCommand(LoadMoreResults);
        }




        #endregion Constructors

        #region Events
        public event EventHandler InvalidateLayout;
        #endregion Events

        #region Fields
        private bool _hasHinsVisible;
        private LayoutState _currentState = LayoutState.Empty;
        private List<Issue> issueList;
        private List<SearchCode> codeList;
        private List<User> userList;
        private List<Repository> repositoryList;
        private string _searchText;
        private string _emptyViewText;
        private bool _hasSearchResults;
        private ObservableCollection<SearchResultContainer> _searchResultList;
        private SearchTypeEnum _selection;
        private string _issuesCount;
        private string _repositoriesCount;
        private string _usersCount;
        private string _codeCount;
        private List<string> _searchHintList;
        private string _selectedSearchItem;
        private bool _expandFilter;
        private string _selectedHint;
        private SearchResultContainer _selectedResult;
        private SearchIssuesRequest issuesRequest;
        private SearchRepositoriesRequest repositoryRequest;
        private SearchCodeRequest codeRequest;
        private SearchUsersRequest userRequest;
        private int _loadTresholdLimit = 10;
        #endregion Fields
        #region Services
        private readonly ICacheService _cacheService;
        private readonly IPopupNavigationService _popupNavigationServce;

        #endregion Services

        #region Properties
        public bool HasHinsVisible
        {
            get => _hasHinsVisible;
            set => SetProperty(ref _hasHinsVisible, value);
        }

        public int LoadTresholdLimit
        {
            get => _loadTresholdLimit;
            set => SetProperty(ref _loadTresholdLimit, value);
        }

        public SearchResultContainer SelectedResult
        {
            get => _selectedResult;
            set => SetProperty(ref _selectedResult, value);
        }


        public bool ExpandFilter
        {
            get => _expandFilter;
            set => SetProperty(ref _expandFilter, value);
        }

        public string SelectedSearchItem
        {
            get => _selectedSearchItem;
            set => SetProperty(ref _selectedSearchItem, value);
        }

        public List<string> SearchHintList
        {
            get => _searchHintList;
            set => SetProperty(ref _searchHintList, value);
        }

        public string CodeCount
        {
            get => _codeCount;
            set => SetProperty(ref _codeCount, value);
        }

        public string UsersCount
        {
            get => _usersCount;
            set => SetProperty(ref _usersCount, value);
        }

        public string RepositoriesCount
        {
            get => _repositoriesCount;
            set => SetProperty(ref _repositoriesCount, value);
        }

        public string IssuesCount
        {
            get => _issuesCount;
            set => SetProperty(ref _issuesCount, value);
        }

        public bool HasSearchResults
        {
            get => _hasSearchResults;
            set {

                SetProperty(ref _hasSearchResults, value);
            }
        }

        public string SelectedHint
        {
            get => _selectedHint;
            set
            {
                SetProperty(ref _selectedHint, value);
                SearchText = value;
            }
        }

        public SearchTypeEnum Selection
        {
            get => _selection;
            set
            {
                if (value != _selection)
                {
                    SetProperty(ref _selection, value);
                }
            }
        }
        public ObservableCollection<SearchResultContainer> SearchResultList
        {
            get => _searchResultList;
            set => SetProperty(ref _searchResultList, value);
        }

        public string EmptyViewText
        {
            get => _emptyViewText;
            set => SetProperty(ref _emptyViewText, value);
        }

        public string SearchText { get => _searchText; set => SetProperty(ref _searchText, value); }
        public LayoutState CurrentState
        {
            get => _currentState;
            set => SetProperty(ref _currentState, value);
        }
       
        #endregion Properties

        #region Commands
        public IMvxCommand CancelSearchCommand { get; }
        public IMvxCommand SearchCommand { get; }

        public IMvxAsyncCommand<SearchTypeEnum> FilterCommand { get; }
        public IMvxAsyncCommand SearchEnteredCommand { get; }
        public IMvxAsyncCommand SelectedResultCommand { get; }
        public IMvxAsyncCommand LoadMoreResultsCommand { get; }
        #endregion Commands

        #region Private Methods

        private string FormatNumber(long num)
        {
            if (num >= 100000000)
            {
                return (num / 1000000D).ToString("0.#M");
            }
            if (num >= 1000000)
            {
                return (num / 1000000D).ToString("0.##M");
            }
            if (num >= 100000)
            {
                return (num / 1000D).ToString("0.#k");
            }
            if (num >= 10000)
            {
                return (num / 1000D).ToString("0.##k");
            }

            return num.ToString("#,0");
        }
        private  async Task ChangeDisplayedList( SearchTypeEnum param)
        {
           await  BusyManager.SetBusy();
           
            if (SearchResultList == null)
            {
                SearchResultList = new ObservableCollection<SearchResultContainer>();
            }
            SearchResultList.Clear();
            if (param == SearchTypeEnum.None) return;
            Selection = param;
            switch (Selection)
            {
                case SearchTypeEnum.Issues:
                    if (issueList != null && issueList.Any())
                    {
                        CreateIssuesSearchResultList(issueList).ForEach(x => SearchResultList.Add(x));
                    }

                    break;
                case SearchTypeEnum.Repository:
                    if (repositoryList != null && repositoryList.Any())
                    {
                        CreateRepositorySearchResultList(repositoryList).ForEach(x => SearchResultList.Add(x));
                    }

                    break;
                case SearchTypeEnum.Code:
                    if (codeList != null && codeList.Any())
                    {
                        CreateCodeSearchResultList(codeList).ForEach(x => SearchResultList.Add(x));
                    }

                    break;
                case SearchTypeEnum.Users:
                    if (userList != null && userList.Any())
                    {
                        CreateUsersSearchResultList(userList).ForEach(x => SearchResultList.Add(x));
                    }

                    break;
                default:
                    return;
            }
            if (SearchResultList != null && SearchResultList.Count < 100)
            {
                LoadTresholdLimit = -1;
            }
            else
            {
                LoadTresholdLimit = 10;
            }
            await BusyManager.SetUnBusy();

        }

        private IReadOnlyList<SearchResultContainer> CreateUsersSearchResultList(IReadOnlyList<User> list)
        {
            return list.Select(x => new SearchResultContainer
            {
                Id = x.Id,
                Title = x.Login,
                Type = SearchTypeEnum.Users,
                Image = ImageSource.FromUri(new Uri(x.AvatarUrl)),
                Discription = x.Bio,
                MoreInfo = string.Format("{0}  {1}", x.Location, x.Email)
            }).ToList();
        }

        private IReadOnlyList<SearchResultContainer> CreateCodeSearchResultList(IReadOnlyList<SearchCode> list)
        {
            return list.Select(x => new SearchResultContainer
            {
                Sha = x.Sha,
                Title = x.Repository.Owner.Login,
                Type = SearchTypeEnum.Code,
                Image = ImageSource.FromUri(new Uri(x.Repository.Owner.AvatarUrl)),
                MoreInfo = x.Path,
                Discription = x.Repository.FullName
            }).ToList();
        }

        private IReadOnlyList<SearchResultContainer> CreateRepositorySearchResultList(IReadOnlyList<Repository> list)
        {
            return list.Select(x => new SearchResultContainer
            {
                Id = x.Id,
                Title = x.FullName,
                Discription = x.Description,
                Type = SearchTypeEnum.Repository,
                StargazersCount = x.StargazersCount,
                Image = "book",
                CodeType = x.Language,
                DateText = x.UpdatedAt.Date.ToShortDateString()
            }).ToList();
        }

        private IReadOnlyList<SearchResultContainer> CreateIssuesSearchResultList(IReadOnlyList<Issue> list)
        {
            return list.Select(x => new SearchResultContainer
            {
                Id = x.Id,
                Title = x.HtmlUrl.Replace("https://github.com/", string.Empty),
                Type = SearchTypeEnum.Issues,
                Image = "exclamation",
                Discription = x.Title,
                DateText = string.Format("{0} opened {1}  {2} comments ", x.User.Login, x.CreatedAt.Date.ToShortDateString(), x.Comments),
                MoreInfo = string.Join(" | ", x.Labels?.Select(y => y.Name).ToArray())
            }).ToList();
        }

        private async Task ShowResult()
        {
            if (SelectedResult != null)
            {
                switch ((SearchTypeEnum)Selection)
                {

                    case SearchTypeEnum.Issues:
                        await MvxNavigationService.Navigate<IssuePageModel, Issue>(issueList.FirstOrDefault(x => x.Id == SelectedResult.Id));

                        break;
                    case SearchTypeEnum.Repository:
                        await MvxNavigationService.Navigate<RepositoryPageModel, Repository>(repositoryList.FirstOrDefault(x => x.Id == SelectedResult.Id));
                        break;
                    case SearchTypeEnum.Code:
                        SearchCode code = codeList.FirstOrDefault(x => x.Sha == SelectedResult.Sha);
                        byte[] content = await GitHubClientService.Repository.Content.GetRawContent(code.Repository.Owner.Login, code.Repository.Name, code.Path);
                        string result = System.Text.Encoding.UTF8.GetString(content);
                        await _popupNavigationServce.PushCodePopup(result);
                        break;
                    case SearchTypeEnum.Users:
                        await MvxNavigationService.Navigate<GithubUserPageModel, User>(userList.FirstOrDefault(x => x.Id == SelectedResult.Id));
                        break;
                }
                SelectedResult = null;
            }
        }

        public void UpdateCacheSearchList(string cacheKey, string item)
        {
            List<string> list = _cacheService.GetCached<List<string>>(cacheKey);
            if (list != null)
            {
                if (!list.Contains(item))
                {
                    list.Add(item);
                }
                else
                {
                    return;
                }
            }
            else
            {
                list = new List<string>
                {
                    item
                };
            }
            _cacheService.SetCache<List<string>>(cacheKey, list);

        }
        private async Task SearchGithub()
        {
            if (!string.IsNullOrEmpty(SearchText) && SearchText.Length > 0)
            {
                await BusyManager.SetBusy();
                issuesRequest = new SearchIssuesRequest(SearchText);
                repositoryRequest = new SearchRepositoriesRequest(SearchText);
                codeRequest = new SearchCodeRequest(SearchText);
                userRequest = new SearchUsersRequest(SearchText);

                Task<SearchIssuesResult> issuesTask = GitHubClientService.Search.SearchIssues(issuesRequest);
                Task<SearchRepositoryResult> repositoriesTask = GitHubClientService.Search.SearchRepo(repositoryRequest);
                Task<SearchCodeResult> codeResultTask = GitHubClientService.Search.SearchCode(codeRequest);
                Task<SearchUsersResult> usersTask = GitHubClientService.Search.SearchUsers(userRequest);
                await Task.WhenAll(issuesTask, codeResultTask, repositoriesTask, usersTask);
                issueList = null;
                codeList = null;
                repositoryList = null;
                userList = null;
                if (issuesTask.Result.TotalCount != 0 || codeResultTask.Result.TotalCount != 0 || usersTask.Result.TotalCount != 0 || repositoriesTask.Result.TotalCount != 0)
                {
                    LoadTresholdLimit = 15;
                    await ProcessResults(issuesTask, repositoriesTask, codeResultTask, usersTask);
                }
                else
                {
                    HasSearchResults = false;
                    EmptyViewText = "Nothing found. Try again";
                }
            }
            await BusyManager.SetUnBusy();
            CurrentState = LayoutState.Empty;
        }
        private async Task LoadMoreResults()
        {
            if (SearchResultList != null && SearchResultList.Count < 100)
            {
                LoadTresholdLimit = -1;
            }

            await BusyManager.SetBusy();
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                SearchIssuesResult issues = null;
                SearchRepositoryResult repositoriesTask = null;
                SearchCodeResult codeResultTask = null;
                SearchUsersResult usersTask = null;
                switch ((SearchTypeEnum)Selection)
                {
                    case SearchTypeEnum.Issues:
                        issuesRequest.Page++;
                        issues = await GitHubClientService.Search.SearchIssues(issuesRequest);
                        CreateIssuesSearchResultList(issues.Items).ForEach(x => SearchResultList.Add(x));

                        break;
                    case SearchTypeEnum.Repository:
                        repositoryRequest.Page++;
                        repositoriesTask = await GitHubClientService.Search.SearchRepo(repositoryRequest);
                        CreateRepositorySearchResultList(repositoriesTask.Items).ForEach(x => SearchResultList.Add(x));

                        break;
                    case SearchTypeEnum.Code:
                        codeRequest.Page++;
                        codeResultTask = await GitHubClientService.Search.SearchCode(codeRequest);
                        CreateCodeSearchResultList(codeResultTask.Items).ForEach(x => SearchResultList.Add(x));

                        break;
                    case SearchTypeEnum.Users:
                        userRequest.Page++;
                        usersTask = await GitHubClientService.Search.SearchUsers(userRequest);
                        CreateUsersSearchResultList(usersTask.Items).ForEach(x => SearchResultList.Add(x));
                        break;
                }
                AssignLists(issues?.Items, codeResultTask?.Items, usersTask?.Items, repositoriesTask?.Items);
                SelectedResult = null;
            }
            await BusyManager.SetUnBusy();

            HasHinsVisible = false;

        }



        private async Task ProcessResults(Task<SearchIssuesResult> issuesTask, Task<SearchRepositoryResult> repositoriesTask, Task<SearchCodeResult> codeResultTask, Task<SearchUsersResult> usersTask)
        {
            UpdateCacheSearchList(StorageKeys.SEARCH_CACHE, SearchText);
            ExpandFilter = true;
            IssuesCount = FormatNumber(issuesTask.Result.TotalCount);
            CodeCount = FormatNumber(codeResultTask.Result.TotalCount);
            UsersCount = FormatNumber(usersTask.Result.TotalCount);
            RepositoriesCount = FormatNumber(repositoriesTask.Result.TotalCount);
            AssignLists(issuesTask.Result.Items, codeResultTask.Result.Items, usersTask.Result.Items, repositoriesTask.Result.Items);
            HasHinsVisible = false;
            HasSearchResults = true;
            InvalidateLayout.Invoke(this, null);
            var selection = issuesTask.Result.TotalCount != 0 ? SearchTypeEnum.Issues
                : codeResultTask.Result.TotalCount != 0 ? SearchTypeEnum.Code
                : usersTask.Result.TotalCount != 0 ? SearchTypeEnum.Users
                : repositoriesTask.Result.TotalCount != 0 ? SearchTypeEnum.Repository
                : SearchTypeEnum.None;
            await ChangeDisplayedList(selection);
        }

        private void AssignLists(IReadOnlyList<Issue> items1, IReadOnlyList<SearchCode> items2, IReadOnlyList<User> items3, IReadOnlyList<Repository> items4)
        {
            AddToList(items1, ref issueList);
            AddToList(items2, ref codeList);
            AddToList(items3, ref userList);
            AddToList(items4, ref repositoryList);
        }

        private void AddToList<T>(IReadOnlyList<T> items1, ref List<T> mainList)
        {
            if (items1 != null)
            {
                if (mainList != null && mainList.Any())
                {
                    mainList.AddRange(items1.ToList());
                }
                else
                {
                    mainList = items1.ToList();
                }
            }
        }
        #endregion Private Methods

        #region Public Methods
        public override async Task Initialize()
        {
            await base.Initialize();
        }
        #endregion PublicMethods
    }


}
