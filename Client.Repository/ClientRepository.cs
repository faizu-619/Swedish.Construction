using Client.Repository.Data.Interfaces;
using Client.Repository.Entities;
using Client.Repository.Repositories.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Repository.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly IClientContext _context;

        public ClientRepository(IClientContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<ClientEntity>> GetClients()
        {
            return await _context
                            .Clients
                            .Find(p => true)
                            .ToListAsync();
        }

        public async Task<ClientEntity> GetClient(string id)
        {
            return await _context
                           .Clients
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync();
        }

        public async Task CreateClient(ClientEntity client)
        {
            await _context.Clients.InsertOneAsync(client);
        }

        public async Task<bool> UpdateClient(ClientEntity client)
        {
            var updateResult = await _context
                                        .Clients
                                        .ReplaceOneAsync(filter: g => g.Id == client.Id, replacement: client);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> DeleteClient(string id)
        {
            FilterDefinition<ClientEntity> filter = Builders<ClientEntity>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context
                                                .Clients
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }
    }
}
