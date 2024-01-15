namespace DrugSchedule.StorageContract.Data;

public class UserContact
{
    public required long UserProfileId { get; set; }

    public UserProfile Profile { get; set; } = default!;

    public required string CustomName { get; set; }
}