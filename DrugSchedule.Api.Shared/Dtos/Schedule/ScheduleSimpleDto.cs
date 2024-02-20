using System;

namespace DrugSchedule.Api.Shared.Dtos;

public class ScheduleSimpleDto
{
    public long Id { get; set; }

    public string? MedicamentName { get; set; }

    public string? MedicamentReleaseFormName { get; set; }

    public string? ThumbnailUrl { get; set; }

    public UserContactSimpleDto? OwnerContact { get; set; }

    public required DateTime CreatedAt { get; set; }

    public required bool Enabled { get; set; }
}