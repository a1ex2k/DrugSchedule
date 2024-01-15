namespace DrugSchedule.Api.Shared.Dtos;

public class NewUserContactDto
{
    public required long UserProfileId { get; set; }

    public string? CustomName { get; set; }
}