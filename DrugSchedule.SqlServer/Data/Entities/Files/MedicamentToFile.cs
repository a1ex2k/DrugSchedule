namespace DrugSchedule.SqlServer.Data.Entities;

public class MedicamentToFile
{
    public required int MedicamentId { get; set; }

    public Medicament? Medicament { get; set; }

    public required long FileInfoId { get; set; }

    public FileInfo? FileInfo { get; set; }
}