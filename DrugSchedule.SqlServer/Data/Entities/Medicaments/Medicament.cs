namespace DrugSchedule.SqlServer.Data.Entities;

public class Medicament
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public int? PackQuantity { get; set; }

    public string? Dosage { get; set; }

    public required int ReleaseFormId { get; set; }

    public MedicamentReleaseForm? ReleaseForm { get; set; }

    public required int ManufacturerId { get; set; }

    public Manufacturer? Manufacturer { get; set; }

    public long? ImageFileInfoId { get; set; }

    public FileInfo? ImageFileInfo { get; set; }
}