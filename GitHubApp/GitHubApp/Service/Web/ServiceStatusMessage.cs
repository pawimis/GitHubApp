using System;

namespace GitHubApp.Service.Web
{
    public class ServiceStatusMessage : BaseMessage
    {
        public ServiceStatusMessage()
        {
        }

        public string ResponseMessage { get; set; }

        public string StatusCode { get; set; }
        public string ErrorCode { get; set; }


        public string Content { get; set; }

        public override string ToString()
        {
            return $"{StatusCode}{Environment.NewLine}{ResponseMessage}";
        }
    }

    public class ServiceStatusMessage<TEntity> : ServiceStatusMessage
    {
        public TEntity Entity { get; set; }

        public bool HasEntity => Entity != null;
    }
}
