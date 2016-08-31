using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace TCCMarketPlace.Model.Logger
{
    //Exception Logger class.
    public class Log4NetLogger
    {
        protected static ILog Logger;

        public Log4NetLogger()
        { 
            log4net.Config.XmlConfigurator.Configure();
            Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);        
        }       

        /// <summary>
        /// Log the exception information.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="ex"> Exception. </param>
        /// <param name="logLevel"> Exception level information. </param>
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
