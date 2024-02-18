using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Services.Models;

public class NewTakingСonfirmation
{
    public long RepeatId { get; set; }

    public DateOnly ForDate { get; set; }

    public TimeOnly ForTime { get; set; }

    public TimeOfDay ForTimeOfDay { get; set; }

    public string? Text { get; set; }
}