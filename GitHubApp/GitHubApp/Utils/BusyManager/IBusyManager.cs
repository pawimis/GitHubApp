using System;
using System.Threading.Tasks;

namespace GitHubApp.Utils.BusyManager
{
    public interface IBusyManager
    {

        event EventHandler<BusyChangedEventArgs> BusyChangedEvent;
        Task SetBusy();
        Task SetUnBusy();
        bool IsBusy { get; }
        bool IsReady { get; }

    }
}
