using FileLoggerTest.Data;
using FileLoggerTest.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System;

namespace FileLoggerTest.Logger
{
    public class DbLogger : ILogger
    {
        private readonly IServiceProvider _serviceProvider;

        public DbLogger(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }
            var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetService<LoggerDbContext>();

            using (var transaction = context.Database.BeginTransaction())
            {
                var log = new LogEntity
                {
                    Message = formatter(state, exception),
                    LogLevel = logLevel
                };

                context.LogEntity.Add(log);

                context.SaveChanges();
                transaction.Commit();
            }
        }
    }
}
