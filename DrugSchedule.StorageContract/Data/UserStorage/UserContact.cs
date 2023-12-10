namespace DrugSchedule.StorageContract.Data;

public class UserContact
{
    public required long UserProfileId { get; set; }

    public required string CustomName { get; set; }

    public bool IsMutual { get; set; }
}