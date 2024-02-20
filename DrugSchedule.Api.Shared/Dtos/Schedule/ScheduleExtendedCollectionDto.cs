using System.Collections.Generic;

namespace DrugSchedule.Api.Shared.Dtos;

public class ScheduleExtendedCollectionDto
{
    public List<TakingScheduleExtendedDto> Schedules { get; set; } = new();
}