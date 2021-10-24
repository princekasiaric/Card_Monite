using CardMon.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CardMon.Infrastructure.Data.ModelConfigurations
{
    public static class APIsServiceLogConfig
    {
        public static void ConfigureAPIsServiceLog(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<APIsServiceLog>(model =>
            {
                model.HasKey(x => x.Id);
                model.Property(x => x.ClientId);
                model.Property(x => x.Method);
                model.Property(x => x.Path);
                model.Property(x => x.Request);
                model.Property(x => x.Response);
                model.Property(x => x.CreatedAt).ValueGeneratedOnAdd().HasDefaultValueSql("GETDATE()");
            });
        }
    }
}
