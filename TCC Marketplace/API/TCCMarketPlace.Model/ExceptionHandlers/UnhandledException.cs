using System;

namespace TCCMarketPlace.Model.ExceptionHandlers
{
    public class UnhandledException : Exception
    {
        public object[] RequestParams { get; set; }
        public string MethodName { get; set; }
        public Exception Exception { get; set; }
        public UnhandledException(Exception exception, string methodName, object[] requestParams) : base()
        {
            Exception = exception;
            RequestParams = requestParams;
            MethodName = methodName;
        }
    }
}
