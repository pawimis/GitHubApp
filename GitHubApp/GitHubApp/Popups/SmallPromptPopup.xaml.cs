
using System;

using Xamarin.Forms.Xaml;

namespace GitHubApp.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SmallPromptPopup : BasePopupPage
    {
        public SmallPromptPopup(string text, string topText = "")
        {
            InitializeComponent();


            TextLabel.Text = text;
            TopTextLabel.Text = topText;
        }
        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            Close();
        }
    }
}