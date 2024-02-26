using System;
using System.Collections.Generic;


namespace DrugSchedule.Api.Shared.Dtos;

public class ScheduleExtendedDto
{
    public long Id { get; set; }

    public UserContactSimpleDto? ContactOwner { get; set; }

    public MedicamentSimpleDto? GlobalMedicament { get; set; }

    public UserMedicamentSimpleDto? UserMedicament { get; set; }

    public string? Information { get; set; }

    public required DateTime CreatedAt { get; set; }

    public required bool Enabled { get; set; }

    public List<ScheduleRepeatDto> ScheduleRepeats { get; set; } = new();

    public List<ScheduleShareExtendedDto>? ScheduleShares { get; set; } = new();
}