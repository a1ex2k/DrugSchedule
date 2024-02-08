﻿using System;

namespace DrugSchedule.Api.Shared.Dtos;

public class UserFullDto
{
    public required long Id { get; set; }

    public required string Username { get; set; }

    public required string Email { get; set; }

    public string? RealName { get; set; }

    public SexDto Sex { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public DownloadableFileDto? Avatar { get; set; }
}