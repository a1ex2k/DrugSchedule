namespace DrugSchedule.SqlServer.Data.Entities;

public class TakingСonfirmation
{
    public long Id { get; set; }

    public required DateTime DateTime { get; set; }

    public long? ImageFileInfoId { get; set; }

    public FileInfo? ImageFileInfo { get; set; }

    public string? Text { get; set; }

    public long? ScheduleRepeatId { get; set; }

    public ScheduleRepeat? ScheduleRepeat { get; set; }
}