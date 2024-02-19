using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Services.Models;

public class TimetableEntry
{
    public DateOnly Date { get; set; }

    public TimeOfDay TimeOfDay { get; set; }

    public TimeOnly? Time { get; set; }

    public long ScheduleId { get; set; } 

    public long RepeatId { get; set; } 

    public long? ConfirmationId { get; set; }
}