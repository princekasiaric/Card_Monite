using CardMon.Core.Interfaces.Repositories;
using CardMon.Core.Models;

namespace CardMon.Infrastructure.Data.Repository
{
    public class APIsServiceLogRepository : Repository<APIsServiceLog>, IAPIsServiceLogRepository
    {
        public APIsServiceLogRepository(AppDbContext context):base(context){}
    }
}
