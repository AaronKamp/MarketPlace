using System;
using System.Collections.Generic;
using System.Security.Claims;
using TCCMarketPlace.Model.Logger;

namespace TCCMarketPlace.Model
{
    public class LoggerEntity
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public ClaimsIdentity User { get; set; }
        public LogLevelEnum Severity { get; set; }
        public Request RequestDetails { get; set; }
        public DateTime RequestTimeStamp { get; set; }
        public DateTime ResponseTimeStamp { get; set; }
        public DateTime ExceptionTimeStamp { get; set; }
        public Exception Exception { get; set; }
        public long ExecutionTime { get; set; }
    }

    public class Request
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public Dictionary<string,object> Arguments { get; set; }
        public string URL { get; set; }
    }
}