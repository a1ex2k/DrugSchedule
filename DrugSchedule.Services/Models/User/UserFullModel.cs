using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.BusinessLogic.Models;

public class UserFullModel
{
    public required long Id { get; set; }

    public required string Username { get; set; }

    public required string Email { get; set; }

    public string? RealName { get; set; }

    public Sex Sex { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public FileInfoModel? Avatar { get; set; }
}