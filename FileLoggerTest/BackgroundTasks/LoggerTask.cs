using FileLoggerTest.Queue;
using Microsoft.Extensions.Hosting;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileLoggerTest.BackgroundTasks
{
    public class LoggerTask : BackgroundService
    {
        private readonly ILogQueue _logQueue;

        public LoggerTask(ILogQueue logQueue)
        {
            _logQueue = logQueue;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            foreach (var logItem in _logQueue.GetLogEntities().GetConsumingEnumerable())
            {

            }
        }
    }
}
