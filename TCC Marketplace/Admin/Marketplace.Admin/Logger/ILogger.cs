
using System;
namespace Logger
{
    public interface ILogger
    {
        void Log(string message, LogLevelEnum logLevel);

        void Log(string message, Exception ex, LogLevelEnum logLevel);
    }    
}
