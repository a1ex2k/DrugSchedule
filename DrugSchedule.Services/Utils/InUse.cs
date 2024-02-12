namespace DrugSchedule.Services.Utils;

public class InUse
{
    public string Message { get; init; }

    public static implicit operator string(InUse error)
    {
        return error.Message;
    }

    public InUse(string message)
    {
        Message = message;
    }
}