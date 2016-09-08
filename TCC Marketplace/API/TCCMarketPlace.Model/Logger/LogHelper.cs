using System;
using System.Text;
namespace TCCMarketPlace.Model.Logger
{
    public static class LogHelper
    {
        const string ExceptionSeperator = "*************************************************************************************************";

        /// <summary>
        /// Create detailed Exception log using Exception object
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="exceptionIdentifier"> Identifier </param>
        /// <returns>Exception info </returns>
        public static string ComposeExceptionLog(Exception ex, Guid exceptionIdentifier)
        {
            var warnExceptionLog = new StringBuilder();
            warnExceptionLog.AppendLine();
            warnExceptionLog.AppendLine(ExceptionSeperator);
            warnExceptionLog.AppendLine(exceptionIdentifier.ToString());
            warnExceptionLog.AppendLine("Exception - " + ex.Message);
            warnExceptionLog.AppendLine(ExceptionSeperator);
            return warnExceptionLog.ToString();
        }

        /// <summary>
        /// Create detailed Exception log using Exception message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exceptionIdentifier"> Identifier</param>
        /// <returns></returns>
        public static string ComposeExceptionLog(string message, Guid exceptionIdentifier)
        {
            var warnExceptionLog = new StringBuilder();
            warnExceptionLog.AppendLine();
            warnExceptionLog.AppendLine(ExceptionSeperator);
            warnExceptionLog.AppendLine(exceptionIdentifier.ToString());
            warnExceptionLog.AppendLine("Exception - " + message);
            warnExceptionLog.AppendLine(ExceptionSeperator);
            return warnExceptionLog.ToString();
        }
    }
}
