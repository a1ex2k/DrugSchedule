using System.Runtime.InteropServices;

namespace DrugSchedule.BusinessLogic.Utils;

public class InvalidInput
{
    public List<string> ErrorsList { get; init; } = new List<string>();

    public bool HasMessages => ErrorsList.Count > 0;

    public void Add(string message)
    {
        ErrorsList.Add(message);
    }

    public static implicit operator string?(InvalidInput? error) 
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

    public InvalidInput(string message)
    {
        ErrorsList.Add(message);
    }

    public InvalidInput()
    {
    }
}