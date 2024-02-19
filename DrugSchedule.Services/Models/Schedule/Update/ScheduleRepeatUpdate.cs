using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Services.Models;

public class ScheduleRepeatUpdate
{
    public long Id { get; set; }

    public required DateOnly BeginDate { get; set; }

    public required TimeOnly? Time { get; set; }

    public required TimeOfDay TimeOfDay { get; set; }

    public required RepeatDayOfWeek RepeatDayOfWeek { get; set; }

    public required DateOnly? EndDate { get; set; }
    
    public string? TakingRule { get; set; }
}