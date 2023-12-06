using DrugSchedule.Api.Data.Entities.Schedule;
using Microsoft.EntityFrameworkCore;

namespace DrugSchedule.Api.Data;

public class DrugScheduleContext : DbContext
{
    public DrugScheduleContext()
    {
    }

    public DrugScheduleContext(DbContextOptions<DrugScheduleContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserProfile>()
            .HasMany(u => u.Contacts)
            .WithOne(c => c.UserProfile);

        modelBuilder.Entity<MedicamentTakingSchedule>()
            .HasMany(s => s.SharedWith)
            .WithMany(c => c.SharedSchedules)
            .UsingEntity<ScheduleShare>();
    }

    public DbSet<Manufacturer> Manufacturers { get; set; }

    public DbSet<Medicament> Medicaments { get; set; }

    public DbSet<MedicamentReleaseForm> Events { get; set; }

    public DbSet<MedicamentTakingSchedule> MedicamentTakingSchedule { get; set; }

    public DbSet<ScheduleShare> ScheduleShare { get; set; }

    public DbSet<Repeat> Repeats { get; set; }

    public DbSet<TakingRule> TakingRules { get; set; }

    public DbSet<TakingСonfirmation> TakingСonfirmations { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<UserMedicament> UserMedicaments { get; set; }

    public DbSet<UserProfile> RoadMapStatuses { get; set; }
    
    public DbSet<UserProfileContact> UserProfileContacts { get; set; }
}