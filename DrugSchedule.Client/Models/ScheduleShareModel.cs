using DrugSchedule.Api.Shared.Dtos;

namespace DrugSchedule.Client.Models;

public class ScheduleShareModel
{
    public long ScheduleId { get; set; }

    public string? Comment { get; set; }

    public UserContactSimpleDto? Contact { get; set; }
}