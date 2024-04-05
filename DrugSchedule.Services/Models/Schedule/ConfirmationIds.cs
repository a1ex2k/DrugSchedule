namespace DrugSchedule.Services.Models;

public record struct ConfirmationIds
{
    public ConfirmationIds(long confirmationId, long repeatId)
    {
        ConfirmationId = confirmationId;
        RepeatId = repeatId;
    }

    public long ConfirmationId { get; init; }

    public long RepeatId { get; init; }
}