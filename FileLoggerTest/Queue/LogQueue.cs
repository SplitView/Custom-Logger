using FileLoggerTest.Models;

using System.Collections.Concurrent;

namespace FileLoggerTest.Queue
{
    public class LogQueue : ILogQueue
    {
        private readonly BlockingCollection<LogEntity> _logEntities = new BlockingCollection<LogEntity>();
        public void Enqueue(LogEntity logEntity)
        {
            _logEntities.Add(logEntity);
        }

        public BlockingCollection<LogEntity> GetLogEntities()
        {
            return _logEntities;
        }
    }
}
