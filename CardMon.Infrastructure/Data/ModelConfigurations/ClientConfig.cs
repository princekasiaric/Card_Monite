using CardMon.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CardMon.Infrastructure.Data.ModelConfigurations
{
    public static class ClientConfig
    {
        public static void ConfigureClient(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(model =>
            {
                model.HasKey(x => x.Id);
                model.HasIndex(x => x.UserName).IsUnique(false);
                model.Property(x => x.IV);
                model.Property(x => x.ApiKey);
                model.Property(x => x.CreatedAt).ValueGeneratedOnAdd().HasDefaultValueSql("GETDATE()");
            });
        }
    }
}
