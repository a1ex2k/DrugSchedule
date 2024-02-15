namespace DrugSchedule.Services.Models.Schedule;

public class ScheduleShareExtended
{
    public required long Id { get; set; }

    public required long ScheduleId { get; set; }

    public UserContactSimple UserContact { get; set; } = default!;

    public string? Comment { get; set; }
}