using FileLoggerTest.Models;

using Microsoft.EntityFrameworkCore;

namespace FileLoggerTest.Data
{
    public class LoggerDbContext : DbContext
    {
        public DbSet<LogEntity> LogEntity { get; set; }
        public LoggerDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}

