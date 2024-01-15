namespace DrugSchedule.Storage.Data.Entities;

public class MedicamentFile
{
    public long Id { get; set; }

    public int MedicamentId { get; set; }

    public Medicament? Medicament { get; set; }

    public required Guid FileGuid { get; set; }

    public FileInfo? FileInfo { get; set; }
}