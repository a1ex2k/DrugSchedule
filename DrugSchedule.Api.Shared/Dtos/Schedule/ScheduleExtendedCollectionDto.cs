using System.Collections.Generic;

namespace DrugSchedule.Api.Shared.Dtos;

public class ScheduleExtendedCollectionDto
{
    public List<ScheduleExtendedDto> Schedules { get; set; } = new();
}