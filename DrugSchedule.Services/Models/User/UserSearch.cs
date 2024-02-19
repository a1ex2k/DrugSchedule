namespace DrugSchedule.Services.Models;

public class UserSearch
{
    public required string UsernameSubstring { get; set; }

    public int Take { get; set; }
}