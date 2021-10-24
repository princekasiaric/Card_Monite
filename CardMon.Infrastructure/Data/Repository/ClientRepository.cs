using CardMon.Core.Interfaces.Repositories;
using CardMon.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CardMon.Infrastructure.Data.Repository
{
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        public ClientRepository(AppDbContext context):base(context){}

        public async Task<Client> GetClientAsync(bool trackChanges) 
            => await FindAll(trackChanges)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync();

        public async Task<Client> GetClientAsync(string apiKey, bool trackChanges) 
            => await FindByCondition(x => x.ApiKey.Equals(apiKey), trackChanges)
            .SingleOrDefaultAsync();

        public async Task<Client> GetClientByIdAsync(int clientId, bool trackChanges) 
            => await FindByCondition(x => x.Id.Equals(clientId), trackChanges)
            .SingleOrDefaultAsync();
    }
}
