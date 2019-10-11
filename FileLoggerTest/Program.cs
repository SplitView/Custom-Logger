using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace FileLoggerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(options =>
            {
                options.UseStartup<Startup>();
            });
    }
}
