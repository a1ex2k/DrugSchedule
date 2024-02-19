using DrugSchedule.Storage.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FileInfo = DrugSchedule.Storage.Data.Entities.FileInfo;

namespace DrugSchedule.Storage.Data;

#nullable disable
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
        var cascadeFKs = modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

        foreach (var fk in cascadeFKs)
        {
            fk.DeleteBehavior = DeleteBehavior.Restrict;
        }

        modelBuilder.Entity<FileInfo>()
            .HasKey(e => e.Guid);

        modelBuilder.Entity<MedicamentFile>()
            .HasOne(e => e.FileInfo)
            .WithMany()
            .HasForeignKey(e => e.FileGuid);

        modelBuilder.Entity<UserProfile>()
            .HasOne(e => e.AvatarInfo)
            .WithMany()
            .HasForeignKey(e => e.AvatarGuid);

        modelBuilder.Entity<UserMedicamentFile>()
            .HasOne(e => e.FileInfo)
            .WithMany()
            .HasForeignKey(e => e.FileGuid);

        modelBuilder.Entity<TakingСonfirmationFile>()
            .HasOne(e => e.FileInfo)
            .WithMany()
            .HasForeignKey(e => e.FileGuid);

        modelBuilder.Entity<UserProfile>()
            .HasOne<IdentityUser>()
            .WithOne()
            .HasForeignKey<UserProfile>(up => up.IdentityGuid)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserProfile>()
            .HasMany(e=> e.Contacts)
            .WithOne(e => e.UserProfile)
            .HasForeignKey(e => e.UserProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserProfile>()
            .HasMany<UserProfileContact>()
            .WithOne(e => e.ContactProfile)
            .HasForeignKey(e => e.ContactProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Medicament>()
            .HasMany(e => e.Files)
            .WithOne(e => e.Medicament)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserMedicament>()
            .HasMany(e => e.Files)
            .WithOne(e => e.UserMedicament)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TakingСonfirmation>()
            .HasMany(e => e.Files)
            .WithOne(e => e.TakingСonfirmation)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MedicamentTakingSchedule>()
            .HasMany(e => e.RepeatSchedules)
            .WithOne(e => e.MedicamentTakingSchedule)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MedicamentTakingSchedule>()
            .HasMany(e => e.ScheduleShares)
            .WithOne(e => e.MedicamentTakingSchedule)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ScheduleRepeat>()
            .HasMany(e => e.TakingСonfirmations)
            .WithOne(e => e.ScheduleRepeat)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserProfileContact>()
            .HasMany(e => e.ScheduleShares)
            .WithOne(e => e.ShareWithContact)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TakingСonfirmation>()
            .Property(e => e.CreatedAt)
            .HasPrecision(2);

        modelBuilder.Entity<TakingСonfirmation>()
            .Property(e => e.ForTime)
            .HasPrecision(2);

        modelBuilder.Entity<MedicamentTakingSchedule>()
            .Property(e => e.CreatedAt)
            .HasPrecision(2);

        modelBuilder.Entity<FileInfo>()
            .Property(e => e.CreatedAt)
            .HasPrecision(2);

        base.OnModelCreating(modelBuilder);
    }


    public DbSet<RefreshTokenEntry> RefreshTokens { get; set; }

    public DbSet<Manufacturer> Manufacturers { get; set; }

    public DbSet<Medicament> Medicaments { get; set; }

    public DbSet<MedicamentReleaseForm> ReleaseForms { get; set; }

    public DbSet<MedicamentTakingSchedule> MedicamentTakingSchedules { get; set; }

    public DbSet<ScheduleShare> ScheduleShare { get; set; }

    public DbSet<ScheduleRepeat> ScheduleRepeat { get; set; }

    public DbSet<TakingСonfirmation> TakingСonfirmations { get; set; }

    public DbSet<UserMedicament> UserMedicaments { get; set; }

    public DbSet<UserProfile> UserProfiles { get; set; }

    public DbSet<UserProfileContact> UserProfileContacts { get; set; }

    public DbSet<FileInfo> FileInfos { get; set; }

    public DbSet<MedicamentFile> MedicamentFiles { get; set; }

    public DbSet<UserMedicamentFile> UserMedicamentFiles { get; set; }

    public DbSet<TakingСonfirmationFile> TakingСonfirmationFiles { get; set; }
}