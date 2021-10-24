using System;
using System.Threading.Tasks;

namespace CardMon.Core.Interfaces.Repositories
{
    public interface IRepositoryManager : IDisposable
    {
        IClientRepository ClientRepository { get; }
        IAPIsServiceLogRepository APIsServiceLogRepository { get; }
        Task SaveChangesAsync();
    }
}
