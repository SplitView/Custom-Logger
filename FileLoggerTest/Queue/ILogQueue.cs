using FileLoggerTest.Models;

using System.Collections.Concurrent;

namespace FileLoggerTest.Queue
{
    public interface ILogQueue
    {
        BlockingCollection<LogEntity> GetLogEntities();
        void Enqueue(LogEntity logEntity);
    }
}
