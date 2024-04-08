using Blazorise;
using DrugSchedule.Api.Shared.Dtos;

namespace DrugSchedule.Client.Models;

public class RepeatModel
{
    public long ScheduleId { get; set; }

    public long RepeatId { get; set; }

    public bool IsNew => RepeatId == default || ScheduleId == default;

    public required DateOnly BeginDate { get; set; }

    public required TimeOnly? Time { get; set; }

    public required TimeOfDayDto TimeOfDay { get; set; }

    public required RepeatDayOfWeekDto RepeatDayOfWeek { get; set; }

    public required DateOnly? EndDate { get; set; }

    public string? TakingRule { get; set; }
}