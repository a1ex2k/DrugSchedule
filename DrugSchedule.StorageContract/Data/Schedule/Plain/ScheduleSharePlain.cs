namespace DrugSchedule.StorageContract.Data;

public class ScheduleSharePlain
{
    public required long ScheduleId { get; set; }

    public required long ShareUserProfileId { get; set; }

    public string? Comment { get; set; }
}