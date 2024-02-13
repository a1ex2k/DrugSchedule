namespace DrugSchedule.StorageContract.Data;

public class ScheduleSharePlain
{
    public long Id { get; set; }

    public required long MedicamentTakingScheduleId { get; set; }

    public required long ShareUserProfileId { get; set; }

    public string? Comment { get; set; }
}