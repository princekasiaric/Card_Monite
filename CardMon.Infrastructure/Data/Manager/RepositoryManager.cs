using Ardalis.GuardClauses;
using CardMon.Core.Interfaces.Repositories;
using CardMon.Infrastructure.Data.Repository;
using System;
using System.Threading.Tasks;

namespace CardMon.Infrastructure.Data.Manager
{
    public class RepositoryManager : IRepositoryManager
    {
        private bool _disposedValue = false;
        private readonly AppDbContext _context;
        private IClientRepository _clientRepository;
        private IAPIsServiceLogRepository _aPIsServiceLogRepository;

        public RepositoryManager(AppDbContext context) 
            => _context = Guard.Against.Null(context, nameof(context));

        public IClientRepository ClientRepository
        {
            get
            {
                if (_clientRepository == null)
                    _clientRepository = new ClientRepository(_context);
                return _clientRepository;
            }
        }

        public IAPIsServiceLogRepository APIsServiceLogRepository
        {
            get
            {
                if (_aPIsServiceLogRepository == null)
                    _aPIsServiceLogRepository = new APIsServiceLogRepository(_context);
                return _aPIsServiceLogRepository;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                // dispose managed resources
                if (disposing && _context != null)
                    _context.Dispose();
                // free unmanaged resources (unmanaged objects) and override finalizer
                // set large fields to null
                _disposedValue = true;
            }
        }

        public async Task SaveChangesAsync() 
            => await _context.SaveChangesAsync();

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        // override finalizer only if 'Dispose(bool disposing)' has code to free unmanage resources
        ~RepositoryManager()
        {
            Dispose(disposing: false);
        }
    }
}
