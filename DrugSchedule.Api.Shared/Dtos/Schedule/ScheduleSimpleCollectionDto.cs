using System.Collections.Generic;
using DrugSchedule.Api.Shared.Dtos;

namespace DrugSchedule.StorageContract.Data;

public class ScheduleSimpleCollectionDto
{
    public List<ScheduleSimpleDto> Schedules { get; set; } = new ();
}