using System;

namespace DrugSchedule.StorageContract.Data;

public class ScheduleRepeatPlain
{
    public long Id { get; set; }

    public DateOnly BeginDate { get; set; }

    public TimeOnly? Time { get; set; }

    public TimeOfDay TimeOfDay { get; set; }

    public RepeatDayOfWeek RepeatDayOfWeek { get; set; }

    public DateOnly? EndDate { get; set; }

    public long MedicamentTakingScheduleId { get; set; }

    public string? TakingRule { get; set; }
}