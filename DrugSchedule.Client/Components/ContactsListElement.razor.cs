using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Constants;
using Microsoft.AspNetCore.Components;

namespace DrugSchedule.Client.Components;

public partial class ContactsListElement
{
    [Parameter, EditorRequired] public UserContactSimpleDto Contact { get; set; } = default!;

    [Parameter] public bool Navigable { get; set; } = true;
    [Parameter] public bool Selectable { get; set; } = true;
    [Parameter] public EventCallback<UserContactSimpleDto> OnSelect { get; set; }
    [Parameter] public string SelectButtonText { get; set; } = "Select";

    private void Navigate()
    {
        if (!Navigable) return;
        NavigationManager.NavigateTo(Routes.Contacts, "id", Contact.UserProfileId.ToString());
    }

    private async Task SelectAsync()
    {
        if (!Selectable || !OnSelect.HasDelegate) return;
        await OnSelect.InvokeAsync(Contact);
    }
}