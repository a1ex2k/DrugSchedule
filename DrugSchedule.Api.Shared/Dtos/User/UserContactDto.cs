using System;

namespace DrugSchedule.Api.Shared.Dtos;

public class UserContactDto
{
    public required long Id { get; set; }

    public required string Username { get; set; }

    public required string СontactName { get; set; }

    public required string? RealName { get; set; }

    public DownloadableFileDto? Avatar { get; set; }
}