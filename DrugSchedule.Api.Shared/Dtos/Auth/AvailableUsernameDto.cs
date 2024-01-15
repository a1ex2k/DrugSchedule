namespace DrugSchedule.Api.Shared.Dtos;

public class AvailableUsernameDto
{
    public required string Username { get; set; }

    public required bool IsAvailable { get; set; }

    public required string Comment { get; set; }
}