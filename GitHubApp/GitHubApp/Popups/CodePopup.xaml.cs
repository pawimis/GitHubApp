using Xamarin.Forms.Xaml;

namespace GitHubApp.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CodePopup : BasePopupPage
    {
        public CodePopup(string text)
        {
            InitializeComponent();
            codeLabel.Text = text;
        }

        private void ImageButton_Clicked(object sender, System.EventArgs e)
        {
            Close();

        }
    }
}