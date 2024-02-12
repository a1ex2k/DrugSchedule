namespace DrugSchedule.Services.Models;

public class PublicUser
{
    public required long Id { get; set; }

    public required string Username { get; set; }

    public string? RealName { get; set; }

    public string? ThumbnailUrl { get; set; }
}