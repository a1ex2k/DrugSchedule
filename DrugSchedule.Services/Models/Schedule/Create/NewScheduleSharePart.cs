namespace DrugSchedule.Services.Models;

public class NewScheduleSharePart
{
    public required long CommonContactProfileId { get; set; }

    public string? Comment { get; set; }
}