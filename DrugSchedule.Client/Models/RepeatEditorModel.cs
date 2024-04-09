using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Utils;

namespace DrugSchedule.Client.Models;

public class RepeatModel
{
    public long ScheduleId { get; set; }

    public long RepeatId { get; set; }

    public bool IsNew => RepeatId == default || ScheduleId == default;

    public DateOnly BeginDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    public TimeSpan Time { get; set; } = TimeSpan.FromHours(12);

    public TimeOfDayDto TimeOfDay { get; set; }

    public DateOnly? EndDate { get; set; }

    public string? TakingRule { get; set; }

    public FlagEnumElement<RepeatDayOfWeekDto>[] Days { get; set; } = ((RepeatDayOfWeekDto)0).ToArray();
}