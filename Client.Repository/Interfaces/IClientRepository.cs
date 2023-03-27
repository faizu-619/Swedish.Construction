using Client.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Repository.Repositories.Interfaces
{
    public interface IClientRepository
    {
        Task<IEnumerable<ClientEntity>> GetClients();
        Task<ClientEntity> GetClient(string id);
        Task CreateClient(ClientEntity client);
        Task<bool> UpdateClient(ClientEntity client);
        Task<bool> DeleteClient(string id);
    }
}
