namespace DrugSchedule.BusinessLogic.Models;

public class UserContact
{
    public required long Id { get; set; }

    public required string Username { get; set; }

    public required string СontactName { get; set; }

    public required string? RealName { get; set; }

    public bool IsCommon { get; set; }

    public bool HasSharedWith { get; set; }

    public bool HasSharedBy { get; set; }

    public DownloadableFile? Avatar { get; set; }
}