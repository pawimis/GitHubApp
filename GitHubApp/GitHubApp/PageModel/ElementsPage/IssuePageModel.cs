using GitHubApp.Interface;
using GitHubApp.Model;
using GitHubApp.PageModel.Base;

using MvvmCross.Navigation;

using Octokit;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubApp.PageModel.ElementsPage
{

    public class IssuePageModel : BasePageModel<Issue>
    {
        #region Constructors
        public IssuePageModel(IMvxNavigationService mvxNavigationService, IGithubService githubService) : base(mvxNavigationService)
        {
            _webService = githubService;
        }
        #endregion Constructors

        #region Events
        #endregion Events

        #region Fields
        private Issue _issue;
        private List<Comment> _comments;

        #endregion Fields

        #region Services
        private IGithubService _webService { get; }
        #endregion Services

        #region Properties

        public List<Comment> Comments
        {
            get => _comments;
            set => SetProperty(ref _comments, value);
        }

        public Issue Issue
        {
            get => _issue;
            set => SetProperty(ref _issue, value);
        }
        #endregion Properties

        #region Commands
        #endregion Commands

        #region Private Methods

        #endregion Private Methods

        #region Public Methods
        public override void Prepare(Issue parameter)
        {
            base.Prepare();
            Issue = parameter;
        }
        public override async Task Initialize()
        {
            await base.Initialize();
            await BusyManager.SetBusy();
            if (Issue != null)
            {
                Service.Web.ServiceStatusMessage<List<Comment>> comments = await _webService.GetIssueComments(Issue.CommentsUrl);
                if (comments.DidSucceed && comments.HasEntity)
                {
                    Comments = comments.Entity.OrderBy(x => x.CreatedAt).ToList();

                }
            }
            await BusyManager.SetUnBusy();

        }
        #endregion PublicMethods
    }


}
