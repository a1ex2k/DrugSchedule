namespace DrugSchedule.Api.Shared.Dtos;


public class ScheduleShareUpdateDto
{
    public required long ScheduleId { get; set; }

    public required long CommonContactProfileId { get; set; }

    public string? Comment { get; set; }
}