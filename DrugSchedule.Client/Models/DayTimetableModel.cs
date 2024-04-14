using DrugSchedule.Api.Shared.Dtos;

namespace DrugSchedule.Client.Models;

public class DayTimetableModel
{
    public DateOnly Date { get; set; }

    public List<TimetableEntryDto> Entries { get; set; } = new();
}