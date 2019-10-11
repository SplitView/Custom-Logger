using FileLoggerTest.Data;

using Microsoft.Extensions.Logging;

using System;

namespace FileLoggerTest.Logger
{
    public class DbLoggerProvider : ILoggerProvider
    {
        private readonly IServiceProvider _serviceProvider;
        private static readonly DbLogger _logger;

        public DbLoggerProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _logger ?? new DbLogger(_serviceProvider);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
