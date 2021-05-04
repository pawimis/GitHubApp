using GitHubApp.Model;
using GitHubApp.Service.Web;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace GitHubApp.Interface
{
    public interface IGithubService
    {
        Task<ServiceStatusMessage<List<Comment>>> GetIssueComments(string url);
        Task<ServiceStatusMessage<IsEven>> CheckIfIsEven(string number);
    }
}
