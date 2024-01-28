namespace DrugSchedule.BusinessLogic.Models;

public class UserSearch
{
    public required string UsernameSubstring { get; set; }
    public int MaxCount { get; set; }
}