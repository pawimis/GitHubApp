using GitHubApp.Interface;
using GitHubApp.Popups;

using Rg.Plugins.Popup.Services;

using System.Threading.Tasks;

namespace GitHubApp.Service
{
    public class PopupNavigationService : IPopupNavigationService
    {
        public async Task PushCodePopup(string text)
        {
            await PopupNavigation.Instance.PushAsync(new CodePopup(text));
        }

        public async Task PushPopup(bool success, string topText, string subtext, string buttonText = "Close")
        {
            await PopupNavigation.Instance.PushAsync(new PromptPopup(success, topText, subtext, buttonText));
        }

        public async Task PushSmallPopup(string text, string topText = "")
        {
            await PopupNavigation.Instance.PushAsync(new SmallPromptPopup(text, topText));

        }
    }
}
