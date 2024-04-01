using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Constants;
using DrugSchedule.Client.Utils;
using Microsoft.AspNetCore.Components;

namespace DrugSchedule.Client.Components;

public partial class ContactsListElement
{
    [Inject] NavigationManager NavigationManager { get; set; } = default!;

    [Parameter, EditorRequired] public UserContactSimpleDto Contact { get; set; } = default!;

    [Parameter] public bool Navigable { get; set; } = true;
    [Parameter] public bool Selectable { get; set; } = true;
    [Parameter] public EventCallback<UserContactSimpleDto> OnSelect { get; set; }
    [Parameter] public string SelectButtonText { get; set; } = "Select";

    private void Navigate()
    {
        if (!Navigable) return;
        NavigationManager.NavigateWithParameter(Routes.Contacts, "id", Contact.UserProfileId.ToString());
    }

    private async Task SelectAsync()
    {
        if (!Selectable || !OnSelect.HasDelegate) return;
        await OnSelect.InvokeAsync(Contact);
    }
}