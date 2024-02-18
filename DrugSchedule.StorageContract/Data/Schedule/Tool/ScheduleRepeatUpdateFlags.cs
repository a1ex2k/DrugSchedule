namespace DrugSchedule.StorageContract.Data;

public class ScheduleRepeatUpdateFlags
{
    public bool ScheduleId { get; set; }

    public bool BeginDate { get; set; }

    public bool Time { get; set; }

    public bool TimeOfDay { get; set; }

    public bool RepeatDayOfWeek { get; set; }

    public bool EndDate { get; set; }

    public bool TakingRule { get; set; }
}