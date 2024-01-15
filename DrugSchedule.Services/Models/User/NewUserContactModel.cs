namespace DrugSchedule.BusinessLogic.Models;

public class NewUserContactModel
{
    public required long UserProfileId { get; set; }

    public string? CustomName { get; set; }
}