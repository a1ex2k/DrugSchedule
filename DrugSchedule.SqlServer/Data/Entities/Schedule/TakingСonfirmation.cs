namespace DrugSchedule.SqlServer.Data.Entities;

public class TakingСonfirmation
{
    public long Id { get; set; }

    public required DateTime DateTime { get; set; }

    public List<FileInfo> Images { get; set; } = new();

    public string? Text { get; set; }

    public long? ScheduleRepeatId { get; set; }

    public ScheduleRepeat? ScheduleRepeat { get; set; }
}