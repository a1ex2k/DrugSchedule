﻿using DrugSchedule.Api.Shared.Dtos;
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

    [Parameter] public bool DateOnly { get; set; } = false;

    protected override void OnParametersSet()
    {
        Time = TimetableEntry.TimeOfDay == TimeOfDayDto.None
            ? TimetableEntry.Time!.Value.ToShortString()
            : TimetableEntry.TimeOfDay.ToHumanReadableString();

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