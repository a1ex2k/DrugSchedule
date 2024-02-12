namespace DrugSchedule.Services.Models;

public class UserContactSimple
{
    public required long UserProfileId { get; set; }

    public required string СontactName { get; set; }

    public bool IsCommon { get; set; }

    public string? ThumbnailUrl { get; set; }
}