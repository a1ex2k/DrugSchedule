using System;

namespace DrugSchedule.Api.Shared.Dtos;

public class UserContactDto
{
    public long UserProfileId { get; set; }

    public string Username { get; set; } = default!;

    public string СontactName { get; set; } = default!;

    public DateOnly? DateOfBirth { get; set; }

    public SexDto? Sex { get; set; }

    public string? RealName { get; set; }

    public bool IsCommon { get; set; }

    public bool HasSharedWith { get; set; }

    public bool HasSharedBy { get; set; }

    public DownloadableFileDto? Avatar { get; set; }
}