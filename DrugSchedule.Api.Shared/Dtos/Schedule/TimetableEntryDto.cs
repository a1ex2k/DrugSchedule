using System;

namespace DrugSchedule.Api.Shared.Dtos;

public class TimetableEntryDto
{
    public DateOnly Date { get; set; }

    public TimeOfDayDto TimeOfDay { get; set; }

    public TimeOnly? Time { get; set; }

    public long ScheduleId { get; set; } 

    public long RepeatId { get; set; } 

    public long? ConfirmationId { get; set; }
}