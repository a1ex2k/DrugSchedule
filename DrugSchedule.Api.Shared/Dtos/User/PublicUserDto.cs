using System;

namespace DrugSchedule.Api.Shared.Dtos;

public class PublicUserDto
{
    public required long Id { get; set; }

    public required string Username { get; set; }

    public string? RealName { get; set; }

    public DownloadableFileDto? Avatar { get; set; }
}