using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Logger
{
    /// <summary>
    /// Exception logging handler.
    /// </summary>
    internal class Log4NetLogger : ILogger
    {
        protected readonly ILog Logger;

        /// <summary>
        /// Constructor. 
        /// </summary>
        public Log4NetLogger()
        { 
            log4net.Config.XmlConfigurator.Configure();
            Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);        
        }

        /// <summary>
        /// Log the exception.
        /// </summary>
        /// <param name="message"> Exception message. </param>
        /// <param name="logLevel"> Exception level. </param>
        public void Log(string message, LogLevelEnum logLevel)
        {
            switch(logLevel)
            {
                case LogLevelEnum.Debug: Logger.Debug(message); break;
                case LogLevelEnum.Information: Logger.Info(message); break;
                case LogLevelEnum.Warning: Logger.Warn(message); break;
                case LogLevelEnum.Error: Logger.Error(message); break;
                case LogLevelEnum.Fatal: Logger.Fatal(message); break;
            }
        }

        /// <summary>
        /// Log the Exception
        /// </summary>
        /// <param name="message"> Exception message</param>
        /// <param name="ex"> Exception ex.</param>
        /// <param name="logLevel"> Exception level. </param>
        public void Log(string message, Exception ex, LogLevelEnum logLevel)
        {
            switch (logLevel)
            {
                case LogLevelEnum.Debug: Logger.Debug(message, ex); break;
                case LogLevelEnum.Information: Logger.Info(message, ex); break;
                case LogLevelEnum.Warning: Logger.Warn(message, ex); break;
                case LogLevelEnum.Error: Logger.Error(message, ex); break;
                case LogLevelEnum.Fatal: Logger.Fatal(message, ex); break;
            }
        }
    }
}
