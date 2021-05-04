using GitHubApp.Model.Enums;

using Xamarin.Forms;

namespace GitHubApp.Model
{
    public class SearchResultContainer
    {
        public long Id { get; set; }
        public string Sha { get; set; }
        public string Discription { get; set; }
        public int StargazersCount { get; set; }
        public string MoreInfo { get; set; }
        public string DateText { get; set; }
        public string CodeType { get; set; }
        public string Title { get; set; }
        public bool IsIssueClosed { get; set; }
        public SearchTypeEnum Type { get; set; }
        public ImageSource Image { get; set; }
    }
}
