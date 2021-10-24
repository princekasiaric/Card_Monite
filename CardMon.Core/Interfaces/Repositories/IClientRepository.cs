using CardMon.Core.Models;
using System.Threading.Tasks;

namespace CardMon.Core.Interfaces.Repositories
{
    public interface IClientRepository : IRepository<Client>
    {
        Task<Client> GetClientAsync(bool trackChanges);
        Task<Client> GetClientByIdAsync(int clientId, bool trackChanges);
        Task<Client> GetClientAsync(string apiKey, bool trackChanges);
    }
}
