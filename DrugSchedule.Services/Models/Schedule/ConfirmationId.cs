namespace DrugSchedule.Services.Models;

public record struct ConfirmationId
{
    public long ConfirmationSelfId { get; init; }

    public long RepeatId { get; init; }
}