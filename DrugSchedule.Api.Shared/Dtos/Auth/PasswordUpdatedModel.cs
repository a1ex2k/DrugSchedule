namespace DrugSchedule.Api.Shared.Dtos;

public class PasswordUpdatedDto
{
    public required string Username { get; set; }

    public required string Password { get; set; }
}