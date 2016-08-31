using System;
namespace Logger
{
    public interface ILogger
    {
        /// <summary>
        /// Log the exception.
        /// </summary>
        /// <param name="message"> Exception message. </param>
        /// <param name="logLevel"> Exception level. </param>
        void Log(string message, LogLevelEnum logLevel);

        /// <summary>
        /// Log the Exception
        /// </summary>
        /// <param name="message"> Exception message</param>
        /// <param name="ex"> Exception ex.</param>
        /// <param name="logLevel"> Exception level. </param>
        void Log(string message, Exception ex, LogLevelEnum logLevel);
    }    
}
