using DrugSchedule.SqlServer.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DrugSchedule.SqlServer.Data;

public class IdentityContext : IdentityDbContext
{
    public IdentityContext()
    {
    }

    public IdentityContext(DbContextOptions<DrugScheduleContext> options) : base(options)
    {
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(Constants.ConnectionString);
        }

        base.OnConfiguring(optionsBuilder);
    }

    public DbSet<RefreshTokenEntry> RefreshTokens { get; set; }
}