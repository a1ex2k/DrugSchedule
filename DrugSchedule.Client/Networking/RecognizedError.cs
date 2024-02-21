namespace DrugSchedule.Client.Networking;

public class RecognizedError
{
    public IReadOnlyList<string> Messages { get; init; } = default!;
}