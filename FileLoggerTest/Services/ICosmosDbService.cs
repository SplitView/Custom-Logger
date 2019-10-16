using Microsoft.Azure.Cosmos;

namespace FileLoggerTest.Services
{
    public interface ICosmosDbService
    {

    }
    public class CosmosDbService : ICosmosDbService
    {
        private const string _databaseName = "Log";
        private const string _containerName = "LogTest";
        public CosmosDbService(CosmosClient dbClient,
            string databaseName,
            string containerName)
        {

        }
    }
}
