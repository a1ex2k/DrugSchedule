namespace DrugSchedule.Services.Utils;

public class NotFound
{
    public string Message { get; init; } 

    public static implicit operator string(NotFound error)
    {
        return error.Message;
    }

    public NotFound(string message)
    {
        Message = message;
    }
}