using FileLoggerTest.Logger.Queue;
using FileLoggerTest.Models;

using Microsoft.Extensions.Logging;

using System;

namespace FileLoggerTest.Logger
{
    public class DbLogger : ILogger
    {
        private readonly ILogQueue _logQueue;

        public DbLogger(ILogQueue logQueue)
        {
            _logQueue = logQueue;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            if (logLevel != LogLevel.Debug && logLevel != LogLevel.Trace && logLevel != LogLevel.Information)
            {
                return true;
            }

            return false;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            var log = new LogEntity
            {
                Message = formatter(state, exception),
                LogLevel = logLevel
            };

            _logQueue.Enqueue(log);
        }
    }
}
