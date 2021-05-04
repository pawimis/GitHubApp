using Rg.Plugins.Popup.Pages;

namespace GitHubApp.Popups
{
    public class BasePopupPage : PopupPage
    {
        #region Protected Metdhos

        protected async void Close()
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
        }
        #endregion Protected Metdhos
    }
}
