using System;

namespace TCCMarketPlace.Model.ExceptionHandlers
{
    public class UnhandledException : Exception
    {
        //Parameters of the method where exception occurred.
        public object[] RequestParams { get; set; }

        //Method where exception occurred.
        public string MethodName { get; set; }

        //The Exception.
        public Exception Exception { get; set; }

        /// <summary>
        /// constructor to initialize class members
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="methodName"></param>
        /// <param name="requestParams"></param>
        public UnhandledException(Exception exception, string methodName, object[] requestParams) : base()
        {
            Exception = exception;
            RequestParams = requestParams;
            MethodName = methodName;
        }
    }
}
