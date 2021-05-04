using GitHubApp.Interface;
using GitHubApp.Model;
using GitHubApp.PageModel.Base;

using MvvmCross.Commands;
using MvvmCross.Navigation;

using Octokit;

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace GitHubApp.PageModel.ElementsPage
{

    public class RepositoryPageModel : BasePageModel<Repository>
    {
        #region Constructors
        public RepositoryPageModel(IMvxNavigationService navigationService, IPopupNavigationService popupNavigationServce) : base(navigationService)
        {
            NavigateFurtherCommand = new MvxAsyncCommand<RepositoryContent>(async (param) => await RepoItemClicked(param));
            _popupNavigationServce = popupNavigationServce;
            pathDirnameStack = new Stack<PathDirContainer>();
            NavigateToDirectoryCommand = new MvxAsyncCommand<string>(async (param) => await NavigateToDirectory(param));
        }




        #endregion Constructors

        #region Events
        #endregion Events

        #region Fields
        private List<RepositoryContent> _repositoryContent;
        private Repository _repo;
        private HtmlWebViewSource _readmeSource;
        private string _path;
        private Stack<PathDirContainer> pathDirnameStack;
        private List<PathDirContainer> _repositoryDirectoryList;
        private string _redame;
        #endregion Fields

        #region Services
        private readonly IPopupNavigationService _popupNavigationServce;

        #endregion Services

        #region Properties

        public string Readme { get => _redame; set => SetProperty(ref _redame, value); }

        public List<PathDirContainer> RepositoryDirectoryList
        {
            get => _repositoryDirectoryList;
            set => SetProperty(ref _repositoryDirectoryList, value);
        }

        public string Path
        {
            get => _path;
            set => SetProperty(ref _path, value);
        }

        public Repository Repo
        {
            get => _repo;
            set => SetProperty(ref _repo, value);
        }

        public HtmlWebViewSource ReadmeSource
        {
            get => _readmeSource;
            set => SetProperty(ref _readmeSource, value);
        }

        public List<RepositoryContent> RepositoryContent
        {
            get => _repositoryContent;
            set => SetProperty(ref _repositoryContent, value);
        }

        #endregion Properties

        #region Commands
        public IMvxAsyncCommand<RepositoryContent> NavigateFurtherCommand { get; }
        public IMvxAsyncCommand<string> NavigateToDirectoryCommand { get; }
        #endregion Commands

        #region Private Methods
        private async Task NavigateToDirectory(string param)
        {
            await BusyManager.SetBusy();

            while (pathDirnameStack.Peek().FullPath != param)
            {
                pathDirnameStack.Pop();
            }
            if (pathDirnameStack.Count() == 1)
            {
                await FetchData();
            }
            else
            {
                PathDirContainer element = pathDirnameStack.Pop();
                pathDirnameStack.Push(new PathDirContainer(element.FullPath, element.Name));
                Path = element.FullPath;
                RepositoryDirectoryList = pathDirnameStack.Reverse().ToList();
                IReadOnlyList<RepositoryContent> content = await GitHubClientService.Repository.Content.GetAllContents(Repo.Owner.Login, Repo.Name, Path);
                if (content != null)
                {
                    RepositoryContent = new List<RepositoryContent>(content.OrderByDescending(x => x.Type.Value).ThenBy(y => y.Name));
                }
            }
            await BusyManager.SetUnBusy();

        }
        private async Task RepoItemClicked(RepositoryContent param)
        {
            await BusyManager.SetBusy();

            if (param != null)
            {
                if (param.Type == ContentType.Dir)
                {
                    Path = param.Path;
                    pathDirnameStack.Push(new PathDirContainer(Path, param.Name));
                    RepositoryDirectoryList = pathDirnameStack.Reverse().ToList();
                    IReadOnlyList<RepositoryContent> content = await GitHubClientService.Repository.Content.GetAllContents(Repo.Owner.Login, Repo.Name, Path);
                    RepositoryContent = new List<RepositoryContent>(content.OrderByDescending(x => x.Type.Value).ThenBy(y => y.Name));
                }
                else
                {
                    byte[] content = await GitHubClientService.Repository.Content.GetRawContent(Repo.Owner.Login, Repo.Name, param.Path);
                    string result = System.Text.Encoding.UTF8.GetString(content);
                    await _popupNavigationServce.PushCodePopup(result);
                }

            }
            await BusyManager.SetUnBusy();

        }
        #endregion Private Methods

        #region Public Methods
        public override void Prepare(Repository parameter)
        {
            base.Prepare();
            Repo = parameter;
        }
        public override async Task Initialize()
        {
            await base.Initialize();
            await BusyManager.SetBusy();
            if (Repo != null)
            {
                await FetchData();

            }
            await BusyManager.SetUnBusy();

        }

        private async Task FetchData()
        {
            pathDirnameStack.Clear();
            Repo = await GitHubClientService.Repository.Get(Repo.Owner.Login, Repo.Name);
            pathDirnameStack.Push(new PathDirContainer(Repo.Name, Repo.Name));
            RepositoryDirectoryList = pathDirnameStack.Reverse().ToList();

            try
            {
                Readme = await GitHubClientService.Repository.Content.GetReadmeHtml(Repo.Owner.Login, Repo.Name);
            }
            catch { Debug.WriteLine("no redame"); }

            if (!string.IsNullOrWhiteSpace(Readme))
            {
                ReadmeSource = new HtmlWebViewSource() { Html = Readme };
            }
            IReadOnlyList<RepositoryContent> content = await GitHubClientService.Repository.Content.GetAllContents(Repo.Owner.Login, Repo.Name);
            RepositoryContent = new List<RepositoryContent>(content.OrderByDescending(x => x.Type.Value).ThenBy(y => y.Name));
        }
        #endregion PublicMethods
    }


}
