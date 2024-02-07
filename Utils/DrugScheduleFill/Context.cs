using Microsoft.EntityFrameworkCore;

namespace DrugScheduleFill;

public class Context : DbContext
{
    private string _connectionString;
  
    public Context(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString);
    }
    
    public DbSet<Manufacturer> Manufacturers { get; set; }
    public DbSet<FileInfo> FileInfos { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<MedicamentReleaseForm> ReleaseForms { get; set; }
    public DbSet<MedicamentFile> MedicamentFiles { get; set; }
}