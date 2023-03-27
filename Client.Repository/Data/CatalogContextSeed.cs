using Client.Repository.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Client.Repository.Data
{
    public class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<ClientEntity> clientCollection)
        {
            bool existProduct = clientCollection.Find(p => true).Any();
            if (!existProduct)
            {
                clientCollection.InsertManyAsync(GetPreconfiguredProducts());
            }
        }

        private static IEnumerable<ClientEntity> GetPreconfiguredProducts()
        {
            return new List<ClientEntity>()
            {
                new ClientEntity()
                {
                    Id = "602d2149e773f2a3990b47f5",
                    Name = "Mr. X",
                },
                new ClientEntity()
                {
                    Id = "602d2149e773f2a3990b47f6",
                    Name = "Mr. Y",
                }
            };
        }
    }
}
