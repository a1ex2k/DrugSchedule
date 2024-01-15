using System;

namespace DrugSchedule.Api.Shared.Dtos;

public class UserDto
{
    public required long Id { get; set; }

    public required string Username { get; set; }

    public required string Email { get; set; }

    public string? RealName { get; set; }

    public Sex Sex { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public FileInfoDto? Avatar { get; set; }
}