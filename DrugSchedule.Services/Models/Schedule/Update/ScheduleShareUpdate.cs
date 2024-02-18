namespace DrugSchedule.Services.Models;


public class ScheduleShareUpdate
{
    public required long ScheduleId { get; set; }

    public required long CommonContactProfileId { get; set; }

    public string? Comment { get; set; }
}