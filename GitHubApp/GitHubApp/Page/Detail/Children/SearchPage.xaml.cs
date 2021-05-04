using GitHubApp.Page.Base;
using GitHubApp.PageModel.Detail.Children;

using MvvmCross.Forms.Presenters.Attributes;

namespace GitHubApp.Page.Detail.Children
{
    [MvxTabbedPagePresentation(position: TabbedPosition.Tab, Title = "Search", WrapInNavigationPage = false, Icon = "magnify")]
    public partial class SearchPage : BaseContentPage<SearchPageModel>
    {
        public SearchPage()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            (BindingContext.DataContext as SearchPageModel).InvalidateLayout += SearchPage_InvalidateLayout;

        }
        private void SearchPage_InvalidateLayout(object sender, System.EventArgs e)
        {
            this.InvalidateMeasure();
        }
    }
}