namespace DrugSchedule.Storage.Data.Entities;

public class TakingСonfirmationFile
{
    public long Id { get; set; }

    public required long TakingСonfirmationId { get; set; }

    public TakingСonfirmation? TakingСonfirmation { get; set; }

    public required Guid FileGuid { get; set; }

    public FileInfo? FileInfo { get; set; }

}