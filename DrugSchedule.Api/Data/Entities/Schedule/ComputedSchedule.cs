namespace DrugSchedule.Api.Data;

public class ComputedSchedule
{
    public long Id { get; set; }

    public required DateTime DateTime { get; set; }

    public required long ScheduledRepeatId { get; set; }

    public ScheduledRepeat ScheduledRepeat { get; set; }
}