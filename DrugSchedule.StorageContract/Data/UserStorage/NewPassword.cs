using System;

namespace DrugSchedule.StorageContract.Data;

public class NewPassword
{
    public required Guid Guid { get; set; }

    public required string Username { get; set; }

    public required string Password{ get; set; }
}