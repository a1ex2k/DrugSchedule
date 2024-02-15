namespace DrugSchedule.Services.Errors;

public class AccessDenied
{
    public string Message { get; init; }

    public static implicit operator string(AccessDenied error)
    {
        return error.Message;
    }

    public AccessDenied(string message)
    {
        Message = message;
    }
}