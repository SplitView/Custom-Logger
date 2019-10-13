
using FileLoggerTest.Logger.Queue;

using Microsoft.Extensions.Logging;

using System;

namespace FileLoggerTest.Logger
{
    public class DbLoggerProvider : ILoggerProvider
    {
        private readonly ILogQueue _logQueue;
        private static DbLogger _logger;

        public DbLoggerProvider(ILogQueue logQueue)
        {
            _logQueue = logQueue;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _logger ?? (_logger = new DbLogger(_logQueue));
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
