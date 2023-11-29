namespace DrugSchedule.Api.Data;

public class TakingСonfirmation
{
    public long Id { get; set; }

    public required DateTime DateTime { get; set; }

    public required byte[] Image { get; set; }

    public long? ComputedScheduleId { get; set; }

    public ScheduledRepeat? ComputedScheduleRepeat { get; set; }
}