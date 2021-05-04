namespace GitHubApp.Utils.BusyManager
{
    public class BusyChangedEventArgs : System.EventArgs
    {

        public BusyChangedEventArgs(bool busy) : base()
        {
            Busy = busy;
        }


        public bool Busy
        {
            get;
            set;
        }

    }
}
