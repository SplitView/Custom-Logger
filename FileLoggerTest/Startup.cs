using FileLoggerTest.BackgroundTasks;
using FileLoggerTest.Constants;
using FileLoggerTest.Data;
using FileLoggerTest.Logger;
using FileLoggerTest.Queue;
using FileLoggerTest.Services;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json.Serialization;

using System;
using System.Threading.Tasks;

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

            service.AddDbContext<LoggerDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString(AppConstants.ConnectionStrings.LoggerConnection));
            });

            service.AddSingleton<ICosmosDbService>(InitializeCosmosClientInstanceAsync(Configuration.GetSection("CosmosDb")).GetAwaiter().GetResult());
            service.AddSingleton<ILogQueue, LogQueue>();
            service.AddHostedService<DbMigrationTask>();
            service.AddHostedService<LoggerTask>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment environment, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddProvider(new DbLoggerProvider(serviceProvider));

            app.UseRouting();

            app.UseCors("Default");

            app.UseEndpoints(endpoint =>
            {
                endpoint.MapControllers();
                endpoint.MapHealthChecks("/health");
            });
        }

        /// <summary>
        /// Creates a Cosmos DB database and a container with the specified partition key. 
        /// </summary>
        /// <returns></returns>
        private static async Task<CosmosDbService> InitializeCosmosClientInstanceAsync(IConfigurationSection configurationSection)
        {
            string databaseName = configurationSection.GetSection("DatabaseName").Value;
            string containerName = configurationSection.GetSection("ContainerName").Value;
            string account = configurationSection.GetSection("Account").Value;
            string key = configurationSection.GetSection("Key").Value;
            CosmosClientBuilder clientBuilder = new CosmosClientBuilder(account, key);
            CosmosClient client = clientBuilder
                                .WithConnectionModeDirect()
                                .Build();
            CosmosDbService cosmosDbService = new CosmosDbService(client, databaseName, containerName);
            DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            await database.Database.CreateContainerIfNotExistsAsync(containerName, "/logLevel");

            return cosmosDbService;
        }

    }
}
