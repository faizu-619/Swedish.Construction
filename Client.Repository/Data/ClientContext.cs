using Client.Repository.Data.Interfaces;
using Client.Repository.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Client.Repository.Data
{
    public class ClientContext : IClientContext
    {
        public ClientContext(IConfiguration configuration)
        {            
            var client = new MongoClient(configuration.GetConnectionString("DatabaseSettings:ConnectionString"));
            var database = client.GetDatabase(configuration["DatabaseSettings:DatabaseName"]);

            Clients = database.GetCollection<ClientEntity>(configuration["DatabaseSettings:CollectionName"]);
            CatalogContextSeed.SeedData(Clients);
        }

        public IMongoCollection<ClientEntity> Clients { get; }
    }
}
