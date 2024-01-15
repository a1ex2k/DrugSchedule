using System;

namespace DrugSchedule.StorageContract.Data;

public class UserProfile
{
    public long UserProfileId { get; set; }

    public Guid UserIdentityGuid { get; set; }

    public string? RealName { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public Sex Sex { get; set; }

    public Guid? AvatarGuid { get; set; }
}