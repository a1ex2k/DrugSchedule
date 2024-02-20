namespace DrugSchedule.Api.Shared.Dtos;

public class ScheduleShareExtendedDto
{
    public required long Id { get; set; }

    public required long ScheduleId { get; set; }

    public UserContactSimpleDto UserContact { get; set; } = default!;

    public string? Comment { get; set; }
}