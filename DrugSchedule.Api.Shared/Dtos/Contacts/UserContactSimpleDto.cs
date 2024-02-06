namespace DrugSchedule.Api.Shared.Dtos;

public class UserContactSimpleDto
{
    public required long UserProfileId { get; set; }

    public required string СontactName { get; set; }

    public bool IsCommon { get; set; }

    public DownloadableFileDto? Avatar { get; set; }
}