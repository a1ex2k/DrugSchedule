using System.Collections.Generic;

namespace DrugSchedule.Api.Shared.Dtos;

public class TimetableDto
{
    public List<TimetableEntryDto> TimetableEntries { get; set; } = new List<TimetableEntryDto>();
}