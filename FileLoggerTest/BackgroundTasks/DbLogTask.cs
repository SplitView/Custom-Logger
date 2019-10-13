using FileLoggerTest.Data;
using FileLoggerTest.Logger.Queue;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileLoggerTest.BackgroundTasks
{
    public class DbLogTask : BackgroundService
    {
        private readonly ILogQueue _logQueue;
        private Task _backgroundTask;
        private readonly LoggerDbContext _loggerDbContext;
        private readonly CancellationTokenSource _shutDown = new CancellationTokenSource();

        public DbLogTask(ILogQueue logQueue, IServiceProvider serviceProvider)
        {
            _logQueue = logQueue;
            var scope = serviceProvider.CreateScope();
            _loggerDbContext = scope.ServiceProvider.GetRequiredService<LoggerDbContext>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _backgroundTask = Task.Run(async () =>
             {
                 while (!stoppingToken.IsCancellationRequested)
                 {
                     await ProcessLogQueue();
                 }
             }, stoppingToken);

            await _backgroundTask;
        }

        private async Task ProcessLogQueue()
        {
            while (!_shutDown.IsCancellationRequested)
            {
                var logEntity = await _logQueue.DequeueAsync(_shutDown.Token);
                if (logEntity != null)
                {
                    await _loggerDbContext.LogEntity.AddAsync(logEntity);
                    await _loggerDbContext.SaveChangesAsync();
                }
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _shutDown.Cancel();

            await Task.WhenAny(_backgroundTask,
                    Task.Delay(Timeout.Infinite, cancellationToken));
        }
    }
}
