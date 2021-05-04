using GitHubApp.Interface;
using GitHubApp.Model;
using GitHubApp.Service.Web;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace GitHubApp.Service
{
    public class GithubWebService : WebService, IGithubService
    {
        public async Task<ServiceStatusMessage<List<Comment>>> GetIssueComments(string url)
        {
            return await MakeGetRequestReturnObject<List<Comment>>(url, null, null, false);

        }
        public async Task<ServiceStatusMessage<IsEven>> CheckIfIsEven(string number)
        {
            return await MakeGetRequestReturnObject<IsEven>("https://api.isevenapi.xyz/api/", $"/iseven/{number}/", null, false);

        }
    }
}
