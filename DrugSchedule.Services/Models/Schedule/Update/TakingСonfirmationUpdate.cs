using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Services.Models;


public class TakingСonfirmationUpdate
{
    public long Id { get; set; }

    public long RepeatId { get; set; }
    
    public string? Text { get; set; }
}