using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Services.Models;

public class UserFull
{
    public required long Id { get; set; }

    public required string Username { get; set; }

    public required string Email { get; set; }

    public string? RealName { get; set; }

    public Sex Sex { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public DownloadableFile? Avatar { get; set; }
}