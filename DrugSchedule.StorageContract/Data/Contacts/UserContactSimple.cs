namespace DrugSchedule.StorageContract.Data;

public class UserContactSimple
{
    public required long ContactProfileId { get; set; }

    public required string CustomName { get; set; }

    public bool IsCommon { get; set; }

    public FileInfo? Avatar { get; set; }
}