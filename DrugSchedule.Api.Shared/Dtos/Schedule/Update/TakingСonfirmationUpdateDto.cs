using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Api.Shared.Dtos;


public class TakingСonfirmationUpdateDto
{
    public long Id { get; set; }

    public long RepeatId { get; set; }
    
    public string? Text { get; set; }
}