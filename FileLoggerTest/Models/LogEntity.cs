using Microsoft.Extensions.Logging;
using System;

namespace FileLoggerTest.Models
{
    public class LogEntity : BaseEntity<string>
    {
        public LogEntity()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Message { get; set; }
        public LogLevel LogLevel { get; set; }
    }
}
