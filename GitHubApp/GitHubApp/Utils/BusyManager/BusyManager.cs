using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GitHubApp.Utils.BusyManager
{
    public class BusyManager : IBusyManager
    {
        #region Fields
        private DateTime? _lastChangeFromReady;
        private TimeSpan _minimumActiveTimeMillisecs = new TimeSpan(0, 0, 0, 0, 500);
        private Stack<int> _busyStack = new Stack<int>();
        #endregion Fields

        public BusyManager()
        {
        }

        #region Events
        public event EventHandler<BusyChangedEventArgs> BusyChangedEvent;
        #endregion Events

        #region Properties
        public bool IsBusy => _busyStack.Count > 0;
        public bool IsReady => _busyStack.Count == 0;
        #endregion Properties

        #region Public Methods
        public async Task SetBusy()
        {
            _busyStack.Push(1);

            if (_busyStack.Count == 1)
            {
                _lastChangeFromReady = DateTime.Now;
            }

            System.Diagnostics.Debug.WriteLine($"Busy - count={_busyStack.Count}");
            await InvokeBusyChangedEvent();
        }
        public async Task SetUnBusy()
        {
            if (_busyStack.Count > 0)
            {
                _busyStack.Pop();
                if (_busyStack.Count == 0)
                {
                    if (_lastChangeFromReady != null)
                    {
                        if (DateTime.Now.Subtract((DateTime)_lastChangeFromReady) < _minimumActiveTimeMillisecs)
                        {
                            System.Diagnostics.Debug.WriteLine($"Unbusy - throttling");
                            Thread.Sleep(_minimumActiveTimeMillisecs);
                        }
                    }
                }
                System.Diagnostics.Debug.WriteLine($"Unbusy - count={_busyStack.Count}");
                await InvokeBusyChangedEvent();
            }
        }
        #endregion  Public Methods



        #region Private Methods
        private async Task InvokeBusyChangedEvent()
        {
            await Task.Run(() => BusyChangedEvent?.Invoke(this, new BusyChangedEventArgs(_busyStack.Count > 0)));
        }
        #endregion Private Methods


    }
}
