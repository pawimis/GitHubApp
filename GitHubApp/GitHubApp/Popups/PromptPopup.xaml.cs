using System;

using Xamarin.Forms.Xaml;

namespace GitHubApp.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PromptPopup : BasePopupPage
    {
        public PromptPopup(bool success, string topText, string subtext, string buttonText)
        {
            InitializeComponent();
            IconImage.Source = success ? "accept" : "wrong";
            if (string.IsNullOrWhiteSpace(topText))
            {
                TopTextLabel.IsVisible = false;
            }
            else
            {
                TopTextLabel.Text = topText;
            }
            if (string.IsNullOrWhiteSpace(subtext))
            {
                BottomTextLabel.IsVisible = false;
            }
            else
            {
                BottomTextLabel.Text = subtext;

            }
            PopupButton.Text = buttonText;
        }
        private void PopupButton_Clicked(object sender, EventArgs e)
        {
            Close();
        }
    }
}