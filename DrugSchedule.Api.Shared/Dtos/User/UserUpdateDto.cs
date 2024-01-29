using System;

namespace DrugSchedule.Api.Shared.Dtos;

public class UserUpdateDto
{
    public string? RealName { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public SexDto? Sex { get; set; }
}