namespace DrugSchedule.StorageContract.Data;

public class UserContactPlain
{
    public long UserProfileId { get; set; }

    public long ContactProfileId { get; set; }

    public required string CustomName { get; set; }
}