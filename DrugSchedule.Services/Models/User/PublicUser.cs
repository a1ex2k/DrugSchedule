namespace DrugSchedule.BusinessLogic.Models;

public class PublicUser
{
    public required long Id { get; set; }

    public required string Username { get; set; }

    public string? RealName { get; set; }

    public DownloadableFile? Avatar { get; set; }
}