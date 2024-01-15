namespace DrugSchedule.BusinessLogic.Models;

public class PublicUserModel
{
    public required long Id { get; set; }

    public required string Username { get; set; }

    public string? RealName { get; set; }

    public FileInfoModel? Avatar { get; set; }
}