using System;

namespace DrugSchedule.StorageContract.Data;

public class UserIdentity
{
    public Guid Guid { get; set; }

    public string? Username { get; set; }

    public string? Email { get; set; }

    public bool? IsEmailConfirmed { get; set; }
}