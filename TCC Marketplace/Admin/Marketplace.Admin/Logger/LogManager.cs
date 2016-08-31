using System.Configuration;

namespace Logger
{
    /// <summary>
    /// Exception log handler.
    /// </summary>
    public class LogManager
    {
        private static ILogger instance = null;
        private static object instanceLock = new object();

        private LogManager()
        { }

        public static ILogger Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (instanceLock)
                    {
                        if(instance == null)
                        {
                            instance = GetLoggerInstance();
                        }
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// Gets logger instance.
        /// </summary>
        /// <returns> Logger instance.</returns>
        private static ILogger GetLoggerInstance()
        {
            string provider = ConfigurationManager.AppSettings[LogConstants.PROVIDER] as string;
            switch (provider)
            {
                case "log4net": return new Log4NetLogger(); break;
                // add case statements here for additional custom provider implementations
                default: return new Log4NetLogger();
            }
        }
    }
}
