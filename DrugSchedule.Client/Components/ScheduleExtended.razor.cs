using DrugSchedule.Api.Shared.Dtos;
using Microsoft.AspNetCore.Components;

namespace DrugSchedule.Client.Components;

public partial class ScheduleExtended
{
    [Parameter, EditorRequired] public ScheduleExtendedDto Schedule { get; set; } = default!;

    [Parameter, EditorRequired] public List<TimetableEntryDto> UpcomingTimetableEntries { get; set; } = default!;
}