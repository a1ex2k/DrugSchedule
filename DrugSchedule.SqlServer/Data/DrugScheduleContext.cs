using DrugSchedule.SqlServer.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DrugSchedule.SqlServer.Data;

public class DrugScheduleContext : DbContext
{
    public DrugScheduleContext()
    {
    }

    public DrugScheduleContext(DbContextOptions<DrugScheduleContext> options) : base(options)
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserProfile>()
                    .HasMany(u => u.Contacts)
                    .WithOne(c => c.UserProfile);

        modelBuilder.Entity<MedicamentTakingSchedule>()
                    .HasMany(s => s.SharedWith)
                    .WithMany(c => c.SharedSchedules)
                    .UsingEntity<ScheduleShare>();

        modelBuilder.Entity<Medicament>()
                    .HasMany(m => m.Images)
                    .WithMany()
                    .UsingEntity<MedicamentToFile>();

        modelBuilder.Entity<UserMedicament>()
                    .HasMany(m => m.Images)
                    .WithMany()
                    .UsingEntity<UserMedicamentToFile>();

        modelBuilder.Entity<TakingСonfirmation>()
                    .HasMany(m => m.Images)
                    .WithMany()
                    .UsingEntity<TakingСonfirmationToFile>();
    }


    public DbSet<Manufacturer> Manufacturers { get; set; }

    public DbSet<Medicament> Medicaments { get; set; }

    public DbSet<MedicamentReleaseForm> MedicamentReleaseForms { get; set; }

    public DbSet<MedicamentReleaseForm> Events { get; set; }

    public DbSet<MedicamentTakingSchedule> MedicamentTakingSchedule { get; set; }

    public DbSet<ScheduleShare> ScheduleShare { get; set; }

    public DbSet<Repeat> Repeats { get; set; }

    public DbSet<TakingRule> TakingRules { get; set; }

    public DbSet<TakingСonfirmation> TakingСonfirmations { get; set; }

    public DbSet<UserMedicament> UserMedicaments { get; set; }

    public DbSet<UserProfile> UserProfiles { get; set; }
    
    public DbSet<UserProfileContact> UserProfileContacts { get; set; }

    public DbSet<Entities.FileInfo> FileInfos { get; set; }

    public DbSet<MedicamentToFile> MedicamentToFiles { get; set; }

    public DbSet<UserMedicamentToFile> UserMedicamentToFiles { get; set; }

    public DbSet<TakingСonfirmationToFile> TakingСonfirmationToFile { get; set; }
}