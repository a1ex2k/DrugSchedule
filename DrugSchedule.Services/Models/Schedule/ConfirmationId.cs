namespace DrugSchedule.Services.Models;

public record struct ConfirmationId
{
    public ConfirmationId(long id, long repeatId)
    {
        Id = id;
        RepeatId = repeatId;
    }

    public long Id { get; init; }

    public long RepeatId { get; init; }
}