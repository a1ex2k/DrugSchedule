using System;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Api.Shared.Dtos;

public class NewScheduleRepeatDto
{
    public long ScheduleId { get; set; }

    public required DateOnly BeginDate { get; set; }

    public required TimeOnly? Time { get; set; }

    public required TimeOfDayDto TimeOfDay { get; set; }

    public required RepeatDayOfWeekDto RepeatDayOfWeek { get; set; }

    public required DateOnly? EndDate { get; set; }

    public string? TakingRule { get; set; }
}