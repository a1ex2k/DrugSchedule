using Blazorise;
using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Networking;
using Microsoft.AspNetCore.Components;

namespace DrugSchedule.Client.Components;

public partial class ContactsList
{
    [Inject] public IApiClient ApiClient { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;

    [Parameter] public UserContactSimpleDto? SelectedContact { get; set; }
    [Parameter] public EventCallback<UserContactSimpleDto> SelectedContactChanged { get; set; }

    [Parameter] public bool Navigable { get; set; }
    [Parameter] public bool CommonOnly { get; set; }

    private List<UserContactSimpleDto> Contacts { get; set; } = new();


    protected override async Task OnInitializedAsync()
    {
        await LoadContactsAsync();
        await base.OnInitializedAsync();
    }


    private async Task LoadContactsAsync()
    {
        var result = CommonOnly ? await ApiClient.GetCommonContactsAsync() : await ApiClient.GetAllContactsAsync();
        if (result.IsOk)
        {
            Contacts = result.ResponseDto.Contacts;
        }
    }


    private void NavigateAsync(UserContactSimpleDto contact)
    {
        if (!Navigable) return;
        NavigationManager.NavigateTo
    }


    private async Task UpdateContactAsync(ContactElementModel contactElement)
    {
        if (string.IsNullOrWhiteSpace(contactElement.NewContactName)) return;

        var result = await ApiClient.AddOrUpdateContactAsync(new NewUserContactDto {
            UserProfileId = contactElement.ContactExtended!.UserProfileId,
            СontactName = contactElement.NewContactName         
        });


        if (!result.IsOk)
        {
            await NotificationService.Success(string.Join("<br>", result.Messages), "Cannot update name");
            return;
        }

        contactElement.ContactSimple.СontactName = contactElement.NewContactName!;
        contactElement.ContactExtended.СontactName = contactElement.NewContactName!;
        contactElement.NewContactName = null;
        await NotificationService.Success("Contact name updated", "Success");
    }


    private async Task RemoveContactAsync(ContactElementModel contactElement)
    {
        var result = await ApiClient.RemoveContactAsync(new UserIdDto { UserProfileId = contactElement.ContactSimple.UserProfileId });
        if (!result.IsOk)
        {
            await NotificationService.Success(string.Join("<br>", result.Messages), "Cannot remove contact");
            return;
        }

        Contacts.Remove(contactElement);
        await NotificationService.Success("Contact removed", "Success");
    }
}