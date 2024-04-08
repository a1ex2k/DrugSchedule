namespace DrugSchedule.Api.Shared.Dtos;

public class NewScheduleSharePartDto
{
    public required long CommonContactProfileId { get; set; }

    public string? Comment { get; set; }
}