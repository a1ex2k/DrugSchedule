using System;

namespace DrugSchedule.Api.Shared.Dtos;

public class UserFullDto
{
    public long Id { get; set; }

    public string Username { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string? RealName { get; set; }

    public SexDto Sex { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public DownloadableFileDto? Avatar { get; set; }
}