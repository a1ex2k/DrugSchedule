using System;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.StorageContract.Data;

public class UserProfile
{
    public long UserProfileId { get; set; }

    public string? RealName { get; set; }

    public required DateOnly? DateOfBirth { get; set; }

    public FileInfo? Image { get; set; }
}