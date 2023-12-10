namespace DrugSchedule.SqlServer.Data.Entities;

public class UserMedicamentToFile
{
    public required long UserMedicamentId { get; set; }

    public UserMedicament? UserMedicament { get; set; }

    public required long FileInfoId { get; set; }

    public FileInfo? FileInfo { get; set; }
}