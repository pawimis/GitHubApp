using GitHubApp.Model.Enums;

using System;

namespace GitHubApp.Model
{
    public class ContributionContainer
    {
        public ContributionTypeEnum ContributionType { get; set; }
        public DateTime ContributionDate { get; set; }
        public string RepositoryName { get; set; }
    }
}
