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

    private readonly CultureInfo _cultureInfo = new CultureInfo("en-US");

    private const string DateFormat = "ddd, MMM d, yyyy";
    private const string TimeFormat = "HH:mm";


    protected override void OnParametersSet()
    {
        Days = Repeat.RepeatDayOfWeek.ToArray();
        PeriodStart = Repeat.BeginDate.ToString(DateFormat, _cultureInfo);
        PeriodEnd = Repeat.EndDate?.ToString(DateFormat, _cultureInfo);
        Time = Repeat.TimeOfDay == TimeOfDayDto.None 
            ? Repeat.Time!.Value.ToString(TimeFormat)
            : Repeat.TimeOfDay.ToHumanReadableString();

        base.OnParametersSet();
    }


    private FlagEnumElement<RepeatDayOfWeekDto>[] Days { get; set; } = default!;

    private string PeriodStart { get; set; } = default!;

    private string? PeriodEnd { get; set; } = default!;

    private string Time { get; set; } = default!;

    private string GetClass(FlagEnumElement<RepeatDayOfWeekDto> day) =>
        day.Checked ? "badge text-bg-primary" : "badge text-bg-light";
}