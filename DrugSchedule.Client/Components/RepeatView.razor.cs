using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Constants;
using DrugSchedule.Client.Utils;
using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace DrugSchedule.Client.Components;

public partial class RepeatView
{
    [Parameter, EditorRequired] public ScheduleRepeatDto Repeat { get; set; } = default!;

    [Parameter, EditorRequired] public List<TimetableEntryDto> UpcomingTimetable { get; set; } = default!;

    protected override void OnParametersSet()
    {
        Days = Repeat.RepeatDayOfWeek.ToArray();
        PeriodStart = Repeat.BeginDate.ToShortString();
        PeriodEnd = Repeat.EndDate?.ToShortString();
        Time = Repeat.TimeOfDay == TimeOfDayDto.None 
            ? Repeat.Time?.ToShortString()
            : Repeat.TimeOfDay.ToHumanReadableString();

        base.OnParametersSet();
    }


    private FlagEnumElement<RepeatDayOfWeekDto>[] Days { get; set; } = default!;

    private string PeriodStart { get; set; } = default!;

    private string? PeriodEnd { get; set; } = default!;

    private string Time { get; set; } = default!;

    private string GetClass(FlagEnumElement<RepeatDayOfWeekDto> day) =>
        day.Checked ? "badge me-1 text-bg-primary" : "badge me-1 text-bg-secondary";
}