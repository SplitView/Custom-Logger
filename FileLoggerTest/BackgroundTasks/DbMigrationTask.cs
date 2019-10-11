
using FileLoggerTest.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FileLoggerTest.BackgroundTasks
{
    public class DbMigrationTask : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public DbMigrationTask(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<LoggerDbContext>();
            var migrations = await context.Database.GetPendingMigrationsAsync();
            if (migrations.Count() > 0)
            {
                await context.Database.MigrateAsync();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
