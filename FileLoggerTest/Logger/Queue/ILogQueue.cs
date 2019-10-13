using FileLoggerTest.Models;

using System.Threading;
using System.Threading.Tasks;

namespace FileLoggerTest.Logger.Queue
{
    public interface ILogQueue
    {
        void Enqueue(LogEntity logEntity);
        Task<LogEntity> DequeueAsync(CancellationToken cancellationToken);
    }
}
