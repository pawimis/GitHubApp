
using GitHubApp.CustomViews.Contribution;
using GitHubApp.Model;

using Octokit;

using System.Collections.Generic;
using System.Windows.Input;

using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GitHubApp.CustomViews.UserContainer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserView : ContentView
    {
        public UserView()
        {
            InitializeComponent();
            Content.BindingContext = this;
        }
        public static readonly BindableProperty UserProperty = BindableProperty
         .Create(nameof(User), typeof(User), typeof(UserView));

        public static readonly BindableProperty UserRepositoriesProperty = BindableProperty
         .Create(nameof(UserRepositories), typeof(IReadOnlyList<Repository>), typeof(UserView));

        public static readonly BindableProperty ContributionsProperty = BindableProperty
         .Create(nameof(Contributions), typeof(Dictionary<int, List<ContributionContainer>>), typeof(ContributionView));

        public static readonly BindableProperty NavigateToRepositoryCommandProperty = BindableProperty
         .Create(nameof(NavigateToRepositoryCommand), typeof(ICommand), typeof(ContributionView));

        public Dictionary<int, List<ContributionContainer>> Contributions
        {
            get => (Dictionary<int, List<ContributionContainer>>)GetValue(ContributionsProperty); set => SetValue(ContributionsProperty, value);
        }

        public User User
        {
            get => (User)GetValue(UserProperty); set => SetValue(UserProperty, value);
        }

        public IReadOnlyList<Repository> UserRepositories
        {
            get => (IReadOnlyList<Repository>)GetValue(UserRepositoriesProperty); set => SetValue(UserRepositoriesProperty, value);
        }

        private async void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            if (User != null && User.HtmlUrl != null)
            {
                await Browser.OpenAsync(User.HtmlUrl, BrowserLaunchMode.SystemPreferred);

            }
        }

        public ICommand NavigateToRepositoryCommand
        {
            get => (ICommand)GetValue(NavigateToRepositoryCommandProperty);
            set => SetValue(NavigateToRepositoryCommandProperty, value);
        }
    }
}