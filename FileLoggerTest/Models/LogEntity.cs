using Microsoft.Extensions.Logging;

namespace FileLoggerTest.Models
{
    public class LogEntity : BaseEntity<int>
    {
        public string Message { get; set; }
        public LogLevel LogLevel { get; set; }
    }
}
