namespace DrugSchedule.Storage.Data.Entities;

public class TakingСonfirmation
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateOnly ForDate { get; set; }

    public TimeOnly? ForTime { get; set; }
    
    public Contract.TimeOfDay ForTimeOfDay { get; set; }

    public List<TakingСonfirmationFile> Files { get; set; } = new();

    public string? Text { get; set; }

    public long ScheduleRepeatId { get; set; }

    public ScheduleRepeat? ScheduleRepeat { get; set; }
}