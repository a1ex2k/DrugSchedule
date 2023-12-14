namespace DrugSchedule.Api.Shared.Models;

public class UsernameAvailabilityDto
{
    public required string Username { get; set; }

    public required string IsAvailable { get; set; }

    public string? Comment { get; set; }
}