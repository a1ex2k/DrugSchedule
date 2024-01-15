using System;

namespace DrugSchedule.StorageContract.Data.UserStorage;

public class PasswordUpdate
{
    public required Guid IdentityGuid { get; set; }

    public required string OldPassword { get; set; }

    public required string NewPassword { get; set; }
}