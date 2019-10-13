using FileLoggerTest.Models;

using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace FileLoggerTest.Logger.Queue
{
    public class LogQueue : ILogQueue
    {
        private readonly ConcurrentQueue<LogEntity> _jobs = new ConcurrentQueue<LogEntity>();
        private readonly SemaphoreSlim _signal = new SemaphoreSlim(0);
        public LogQueue()
        {
        }

        public void Enqueue(LogEntity logEntity)
        {
            _jobs.Enqueue(logEntity);
            _signal.Release();
        }

        public async Task<LogEntity> DequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            var isdequeued = _jobs.TryDequeue(out var job);

            return isdequeued ? job : null;
        }
    }
}
