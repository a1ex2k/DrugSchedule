using System;

namespace DrugSchedule.Api.Shared.Dtos;

public class TimetableFilterDto
{
    public DateOnly MinDate { get; set; }

    public DateOnly MaxDate { get; set; }

    public long? ScheduleId { get; set; }
}