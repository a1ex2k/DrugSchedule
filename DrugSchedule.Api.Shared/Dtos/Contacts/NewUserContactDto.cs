namespace DrugSchedule.Api.Shared.Dtos;

public class NewUserContactDto
{
    public required long UserProfileId { get; set; }

    public required string СontactName { get; set; }
}