using CardMon.Infrastructure.Data.ModelConfigurations;
using Microsoft.EntityFrameworkCore;

namespace CardMon.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options){}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ConfigureClient();
            modelBuilder.ConfigureAPIsServiceLog();
        }
    }
}
