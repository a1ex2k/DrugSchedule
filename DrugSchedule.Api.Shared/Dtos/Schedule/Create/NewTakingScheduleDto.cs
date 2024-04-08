using System.Collections.Generic;

namespace DrugSchedule.Api.Shared.Dtos;

public class NewScheduleDto
{
    public int? GlobalMedicamentId { get; set; }

    public long? UserMedicamentId { get; set; }

    public string? Information { get; set; }

    public bool Enabled { get; set; }

    public List<NewScheduleSharePartDto>? Shares { get; set; }
}