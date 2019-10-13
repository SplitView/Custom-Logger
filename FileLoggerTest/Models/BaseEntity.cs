using System.ComponentModel.DataAnnotations;

namespace FileLoggerTest.Models
{
    public class BaseEntity<T>
    {
        [Key]
        public T Id { get; set; }
    }
}
