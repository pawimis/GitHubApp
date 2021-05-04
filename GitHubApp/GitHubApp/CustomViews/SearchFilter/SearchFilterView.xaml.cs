using GitHubApp.Model.Enums;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GitHubApp.CustomViews.SearchFilter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchFilterView : ContentView
    {
        public SearchFilterView()
        {
            InitializeComponent();
            Content.BindingContext = this;
        }

        private Color AccentColor = (Color)Application.Current.Resources["Accent"];

        public static readonly BindableProperty IssuesCountProperty = BindableProperty
          .Create(nameof(IssuesCount), typeof(string), typeof(SearchFilterView));

        public static readonly BindableProperty RepositoriesCountProperty = BindableProperty
          .Create(nameof(RepositoriesCount), typeof(string), typeof(SearchFilterView));

        public static readonly BindableProperty UsersCountProperty = BindableProperty
          .Create(nameof(UsersCount), typeof(string), typeof(SearchFilterView));

        public static readonly BindableProperty CodeCountProperty = BindableProperty
          .Create(nameof(CodeCount), typeof(string), typeof(SearchFilterView));

        public static readonly BindableProperty SearchTypeProperty = BindableProperty
          .Create(nameof(SearchType), typeof(SearchTypeEnum), typeof(SearchFilterView), SearchTypeEnum.None, BindingMode.TwoWay, propertyChanged: SearchTypeChanged);


        public string IssuesCount { get => (string)GetValue(IssuesCountProperty); set => SetValue(IssuesCountProperty, value); }
        public string RepositoriesCount { get => (string)GetValue(RepositoriesCountProperty); set => SetValue(RepositoriesCountProperty, value); }
        public string UsersCount { get => (string)GetValue(UsersCountProperty); set => SetValue(UsersCountProperty, value); }
        public string CodeCount { get => (string)GetValue(CodeCountProperty); set => SetValue(CodeCountProperty, value); }
        public SearchTypeEnum SearchType { get => (SearchTypeEnum)GetValue(SearchTypeProperty); set => SetValue(SearchTypeProperty, value); }

        private void TapGestureRecognizer_Tapped_UsersFrame(object sender, System.EventArgs e)
        {
            ResetFrameColor();
            UsersFrame.BorderColor = AccentColor;
            SearchType = SearchTypeEnum.Users;
        }

        private void TapGestureRecognizer_Tapped_IssuesFrame(object sender, System.EventArgs e)
        {
            ResetFrameColor();
            IssuesFrame.BorderColor = AccentColor;
            SearchType = SearchTypeEnum.Issues;
        }

        private void TapGestureRecognizer_Tapped_RepositoriesFrame(object sender, System.EventArgs e)
        {
            ResetFrameColor();
            RepositoriesFrame.BorderColor = AccentColor;
            SearchType = SearchTypeEnum.Repository;
        }

        private void TapGestureRecognizer_Tapped_CodeFrame(object sender, System.EventArgs e)
        {
            ResetFrameColor();
            CodeFrame.BorderColor = AccentColor;
            SearchType = SearchTypeEnum.Code;
        }

        private void ResetFrameColor()
        {
            CodeFrame.BorderColor = RepositoriesFrame.BorderColor = UsersFrame.BorderColor = IssuesFrame.BorderColor = Color.Transparent;
        }

        private static void SearchTypeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != null && newValue is SearchTypeEnum type && bindable is SearchFilterView view)
            {
                view.ResetFrameColor();
                switch (type)
                {
                    case SearchTypeEnum.Issues:
                        view.IssuesFrame.BorderColor = view.AccentColor;
                        break;
                    case SearchTypeEnum.Repository:
                        view.RepositoriesFrame.BorderColor = view.AccentColor;
                        break;
                    case SearchTypeEnum.Code:
                        view.CodeFrame.BorderColor = view.AccentColor;
                        break;
                    case SearchTypeEnum.Users:
                        view.UsersFrame.BorderColor = view.AccentColor;
                        break;
                }
            }

        }
    }
}