namespace DrugSchedule.Services.Errors;

public class NotFound
{
    public List<string> ErrorsList { get; init; } = new List<string>();

    public bool HasMessages => ErrorsList.Count > 0;

    public void Add(string message)
    {
        ErrorsList.Add(message);
    }

    public static implicit operator string?(NotFound? error)
    {
        if (error == null || error.ErrorsList.Count == 0)
        {
            return null;
        }

        if (error.ErrorsList.Count == 1)
        {
            return error.ErrorsList[0];
        }

        return string.Join(" \n", error.ErrorsList);
    }

    public NotFound(string message)
    {
        ErrorsList.Add(message);
    }

    public NotFound()
    {
    }
}