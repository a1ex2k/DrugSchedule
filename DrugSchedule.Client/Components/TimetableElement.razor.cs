using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Constants;
using DrugSchedule.Client.Utils;
using Microsoft.AspNetCore.Components;

namespace DrugSchedule.Client.Components;

public partial class TimetableElement
{
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;

    [Parameter, EditorRequired] public TimetableEntryDto TimetableEntry { get; set; } = default!;

    [Parameter] public bool NavigableToSchedule { get; set; } = false;

    [Parameter] public bool NavigableToConfirmation { get; set; } = true;

    [Parameter] public Mode ShowMode { get; set; } = Mode.DateAndTime;

    public enum Mode { DateAndTime, DateOnly, TimeOnly }


    protected override void OnParametersSet()
    {
        if (TimetableEntry != null)
        {
            Time = TimetableEntry.TimeOfDay == TimeOfDayDto.None
                ? TimetableEntry.Time?.ToShortString()
                : TimetableEntry.TimeOfDay.ToHumanReadableString();
        }


        base.OnParametersSet();
    }

    private string Time { get; set; } = default!;


    private void NavigateToSchedule()
    {
        NavigationManager.NavigateWithParameter(Routes.Schedules, "id", TimetableEntry.ScheduleId);
    }

    private void NavigateToConfirmation()
    {
        NavigationManager.NavigateWithParameter(Routes.Schedules, "id", TimetableEntry.ScheduleId);
    }
}