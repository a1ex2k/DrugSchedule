using System;
using DrugSchedule.Api.Shared.Dtos;

namespace DrugSchedule.Api.Shared.Dtos;

public class ScheduleRepeatDto
{
    public long Id { get; set; }

    public DateOnly BeginDate { get; set; }

    public TimeOnly? Time { get; set; }

    public TimeOfDayDto TimeOfDay { get; set; }

    public RepeatDayOfWeekDto RepeatDayOfWeek { get; set; }

    public DateOnly? EndDate { get; set; }

    public long MedicamentTakingScheduleId { get; set; }

    public string? TakingRule { get; set; }
}