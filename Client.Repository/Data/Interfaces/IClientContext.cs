using Client.Repository.Entities;
using MongoDB.Driver;

namespace Client.Repository.Data.Interfaces
{
    public interface IClientContext
    {
        IMongoCollection<ClientEntity> Clients { get; }
    }
}
