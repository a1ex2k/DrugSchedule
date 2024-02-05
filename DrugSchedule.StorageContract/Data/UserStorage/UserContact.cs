namespace DrugSchedule.StorageContract.Data;

public class UserContact
{
    public UserProfile Profile { get; set; } = default!;

    public required string CustomName { get; set; }

    public bool IsCommon { get; set; }

    public bool HasSharedWith { get; set; }

    public bool HasSharedBy { get; set; }
}