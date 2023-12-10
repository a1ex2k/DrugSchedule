namespace DrugSchedule.StorageContract.Data;

public class NewUserIdentity
{
    public required string Username { get; set; }

    public required string Email { get; set; }

    public required string Password { get; set; }
}