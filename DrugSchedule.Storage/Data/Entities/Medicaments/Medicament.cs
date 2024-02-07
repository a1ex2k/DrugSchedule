namespace DrugSchedule.Storage.Data.Entities;

public class Medicament
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }
    
    public string? Composition { get; set; }
    
    public required int ReleaseFormId { get; set; }

    public MedicamentReleaseForm? ReleaseForm { get; set; }

    public required int? ManufacturerId { get; set; }

    public Manufacturer? Manufacturer { get; set; }

    public List<MedicamentFile> Files { get; set; } = new();
}