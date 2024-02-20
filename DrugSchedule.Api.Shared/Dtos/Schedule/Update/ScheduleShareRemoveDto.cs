namespace DrugSchedule.Api.Shared.Dtos;

public class ScheduleShareRemoveDto
{
    public required long ScheduleId { get; set; }

    public required long CommonContactProfileId { get; set; }
}