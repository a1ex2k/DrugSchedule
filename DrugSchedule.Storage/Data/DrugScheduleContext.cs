using DrugSchedule.Storage.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FileInfo = DrugSchedule.Storage.Data.Entities.FileInfo;

namespace DrugSchedule.Storage.Data;

public class DrugScheduleContext : IdentityDbContext
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
            optionsBuilder.UseSqlServer(string.Empty);
        }

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserProfile>()
            .HasOne<IdentityUser>()
            .WithOne()
            .HasForeignKey<UserProfile>(up => up.IdentityGuid);

        modelBuilder.Entity<FileInfo>()
            .HasKey(u => u.Guid);

        modelBuilder.Entity<MedicamentFile>()
            .HasOne(mf => mf.FileInfo)
            .WithMany()
            .HasForeignKey(mf => mf.FileGuid);

        modelBuilder.Entity<TakingСonfirmationFile>()
            .HasOne<FileInfo>(tcf => tcf.FileInfo)
            .WithMany()
            .HasForeignKey(tcf => tcf.FileGuid);

        modelBuilder.Entity<UserMedicamentFile>()
            .HasOne<FileInfo>(umf => umf.FileInfo)
            .WithMany()
            .HasForeignKey(umf => umf.FileGuid);

        modelBuilder.Entity<UserProfile>()
            .HasOne<FileInfo>()
            .WithMany()
            .HasForeignKey(up => up.AvatarGuid);

        modelBuilder.Entity<UserProfile>()
            .HasMany(u => u.Contacts)
            .WithOne(c => c.UserProfile);

        modelBuilder.Entity<UserProfileContact>()
            .HasOne(x => x.UserProfile)
            .WithMany(x => x.Contacts)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserProfileContact>()
            .HasOne(x => x.ContactProfile);

        modelBuilder.Entity<MedicamentTakingSchedule>()
            .HasMany(s => s.SharedWith)
            .WithMany(c => c.SharedSchedules)
            .UsingEntity<ScheduleShare>();

        base.OnModelCreating(modelBuilder);
    }


    public DbSet<RefreshTokenEntry> RefreshTokens { get; set; }

    public DbSet<Manufacturer> Manufacturers { get; set; }

    public DbSet<Medicament> Medicaments { get; set; }

    public DbSet<MedicamentReleaseForm> ReleaseForms { get; set; }

    public DbSet<MedicamentTakingSchedule> MedicamentTakingSchedule { get; set; }

    public DbSet<ScheduleShare> ScheduleShare { get; set; }

    public DbSet<ScheduleRepeat> Repeats { get; set; }

    public DbSet<TakingСonfirmation> TakingСonfirmations { get; set; }

    public DbSet<UserMedicament> UserMedicaments { get; set; }

    public DbSet<UserProfile> UserProfiles { get; set; }

    public DbSet<UserProfileContact> UserProfileContacts { get; set; }

    public DbSet<FileInfo> FileInfos { get; set; }

    public DbSet<MedicamentFile> MedicamentFiles { get; set; }

    public DbSet<UserMedicamentFile> UserMedicamentFiles { get; set; }

    public DbSet<TakingСonfirmationFile> TakingСonfirmationFiles { get; set; }
}