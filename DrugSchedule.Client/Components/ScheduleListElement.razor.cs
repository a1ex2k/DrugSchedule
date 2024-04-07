using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Constants;
using DrugSchedule.Client.Utils;
using Microsoft.AspNetCore.Components;

namespace DrugSchedule.Client.Components;

public partial class ScheduleListElement
{
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;
    [Parameter, EditorRequired] public ScheduleSimpleDto Schedule { get; set; } = default!;

    [Parameter] public bool Selectable { get; set; } = true;
    [Parameter] public bool Navigable { get; set; } = true;

    [Parameter] public EventCallback<ScheduleSimpleDto> OnSelect { get; set; }

    [Parameter] public string SelectButtonText { get; set; } = "Select";


    private async Task SelectAsync()
    {
        if (!Selectable || !OnSelect.HasDelegate) return;
        await OnSelect.InvokeAsync(Schedule);
    }

    private async Task Navigate()
    {
        if (!Navigable) return;
        NavigationManager.NavigateWithParameter(Routes.UserDrugs, "id", Schedule.Id);
    }
}