using System.Threading.Tasks;

namespace GitHubApp.Interface
{
    public interface IPopupNavigationService
    {
        Task PushPopup(bool success, string topText, string subtext, string buttonText = "Close");
        Task PushSmallPopup(string text, string topText = "");
        Task PushCodePopup(string text);
    }
}
