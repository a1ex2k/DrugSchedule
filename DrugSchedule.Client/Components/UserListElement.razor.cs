using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Constants;
using Microsoft.AspNetCore.Components;

namespace DrugSchedule.Client.Components;

public partial class UserListElement
{
    [Parameter, EditorRequired] public PublicUserDto PublicUser { get; set; } = default!;

    [Parameter] public bool Selectable { get; set; } = true;

    [Parameter] public EventCallback<PublicUserDto> OnSelect { get; set; }

    [Parameter] public string SelectButtonText { get; set; } = "Select";


    private async Task SelectAsync()
    {
        if (!Selectable || !OnSelect.HasDelegate) return;
        await OnSelect.InvokeAsync(PublicUser);
    }
}