using System;
using System.Web;
using Marketplace.Admin.Enums;
using System.Web.Routing;
using Logger;
using System.Collections.Specialized;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Marketplace.Admin.Filters
{
    /// <summary>
    /// Object to transfer log info.
    /// </summary>
    public class LoggerEntity
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public LogLevelEnum Severity { get; set; }
        public Request RequestDetails { get; set; }
        public DateTime RequestTimeStamp { get; set; }
        public DateTime ResponseTimeStamp { get; set; }
        public DateTime  ExceptionTimeStamp { get; set; }
        public Exception Exception { get; set; }
        public long ExecutionTime { get; set; }

    }

    public class Request
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public string URL { get; set; }
        public string UserName { get; set; }
        public Dictionary<string, object> Arguments { get; internal set; }
    }
}