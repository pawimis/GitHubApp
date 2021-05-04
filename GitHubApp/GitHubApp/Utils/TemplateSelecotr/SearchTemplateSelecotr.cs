using GitHubApp.Model;

using Xamarin.Forms;

namespace GitHubApp.Utils.TemplateSelecotr
{
    public class SearchTemplateSelecotr : DataTemplateSelector
    {
        public DataTemplate IssueTemplate { get; set; }
        public DataTemplate RepositoryTemplate { get; set; }
        public DataTemplate UserTemplate { get; set; }
        public DataTemplate CodeTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject bindable)
        {
            if (item is SearchResultContainer container)
            {
                switch (container.Type)
                {

                    case Model.Enums.SearchTypeEnum.Issues:
                        return IssueTemplate;
                    case Model.Enums.SearchTypeEnum.Repository:
                        return RepositoryTemplate;
                    case Model.Enums.SearchTypeEnum.Code:
                        return CodeTemplate;
                    case Model.Enums.SearchTypeEnum.Users:
                        return UserTemplate;
                }
            }
            return null;
        }
    }
}

