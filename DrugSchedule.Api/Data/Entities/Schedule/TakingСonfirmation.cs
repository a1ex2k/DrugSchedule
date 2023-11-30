namespace DrugSchedule.Api.Data;

public class TakingСonfirmation
{
    public long Id { get; set; }

    public required DateTime DateTime { get; set; }

    public required Guid ImageGuid { get; set; }

    public string Text { get; set; }

    public long? ScheduleRepeatId { get; set; }

    public ScheduleRepeat? ScheduleRepeat { get; set; }
}