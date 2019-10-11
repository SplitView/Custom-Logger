using FileLoggerTest.BackgroundTasks;
using FileLoggerTest.Constants;
using FileLoggerTest.Data;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Newtonsoft.Json.Serialization;

namespace FileLoggerTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection service)
        {
            service.AddHealthChecks();

            service.AddControllers()
                   .AddNewtonsoftJson(options =>
                   {
                       options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                   });

            service.AddCors(options =>
            {
                options.AddPolicy("Default", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });

            service.AddDbContextPool<LoggerDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString(AppConstants.ConnectionStrings.LoggerConnection));
            });

            service.AddHostedService<DbMigrationTask>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors("Default");

            app.UseEndpoints(endpoint =>
            {
                endpoint.MapControllers();
                endpoint.MapHealthChecks("/health");
            });
        }

    }
}
