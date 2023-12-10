namespace DrugSchedule.SqlServer.Data.Entities;

public class TakingСonfirmationToFile
{
    public required long TakingСonfirmationId { get; set; }

    public TakingСonfirmation? TakingСonfirmation { get; set; }

    public required long FileInfoId { get; set; }

    public FileInfo? FileInfo { get; set; }
}