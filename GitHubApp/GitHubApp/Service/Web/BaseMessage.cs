using System;

namespace GitHubApp.Service.Web
{
    public abstract class BaseMessage
    {
        protected BaseMessage()
        {
        }

        protected BaseMessage(bool didSucceed, Exception exception)
        {
            DidSucceed = didSucceed;
            RaisedException = exception;
        }


        public bool DidSucceed { get; set; }


        public Exception RaisedException { get; set; }



        public bool HasException => RaisedException != null;
    }
}
