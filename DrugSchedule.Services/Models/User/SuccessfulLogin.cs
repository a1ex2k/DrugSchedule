namespace DrugSchedule.Services.Models;

public class SuccessfulLogin
{
    public required long UserProfileId { get; set; }

    public required Guid UserIdentityGuid { get; set; }

    public required string Username { get; set; }

    public required string Email { get; set; }
}