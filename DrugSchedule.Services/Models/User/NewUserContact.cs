namespace DrugSchedule.BusinessLogic.Models;

public class NewUserContact
{
    public required long UserProfileId { get; set; }

    public string? CustomName { get; set; }
}