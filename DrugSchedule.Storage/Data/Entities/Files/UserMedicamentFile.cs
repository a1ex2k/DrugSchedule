namespace DrugSchedule.Storage.Data.Entities;

public class UserMedicamentFile
{
    public long Id { get; set; }

    public long UserMedicamentId { get; set; }

    public UserMedicament? UserMedicament { get; set; }

    public required Guid FileGuid { get; set; }

    public FileInfo? FileInfo { get; set; }
}