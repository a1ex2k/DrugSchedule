using DrugSchedule.Services.Models;
using DrugSchedule.Services.Models.Schedule;

namespace DrugSchedule.StorageContract.Data;

public class ScheduleSimpleCollection
{
    public List<ScheduleSimple> Schedules { get; set; } = new List<ScheduleSimple>();
}