using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Networking;
using DrugSchedule.Client.Components;
using Microsoft.AspNetCore.Components;

namespace DrugSchedule.Client.ViewModels;

public class ContactsViewModel : PageViewModelBase
{
    [Parameter] public UserContactSimpleDto? SelectedContact { get; set; }
    [Parameter] public EventCallback<UserContactSimpleDto> SelectedContactChanged { get; set; }

    [Parameter] public bool Navigable { get; set; }
    [Parameter] public bool CommonOnly { get; set; }

    private class ContactElementModel
    {
        public required UserContactSimpleDto ContactSimple { get; set; }
        public UserContactDto? ContactExtended { get; set; }
        public string? NewContactName { get; set; }
        public bool IsExpanded { get; set; }
    }

    private List<ContactElementModel> Contacts { get; set; } = new();


    protected override async Task OnInitializedAsync()
    {
        await LoadAllContactsAsync();
        await base.OnInitializedAsync();
    }


    private async Task LoadAllContactsAsync()
    {
        var result = CommonOnly ? await ApiClient.GetCommonContactsAsync() : await ApiClient.GetAllContactsAsync();
        if (result.IsOk)
        {
            Contacts = result.ResponseDto.Contacts
                .Select(x => new ContactElementModel
                {
                    ContactSimple = x
                }).ToList();
        }
    }


    private async Task InvertExpandAsync(ContactElementModel contactElement)
    {
        if (contactElement.IsExpanded)
        {
            contactElement.IsExpanded = false;
            contactElement.ContactExtended = null;
            return;
        }

        if (contactElement.ContactExtended != null)
        {
            contactElement.IsExpanded = true;
            return;
        }

        var result = await ApiClient.GetSingleExtendedContactAsync(new UserIdDto { UserProfileId = contactElement.ContactSimple.UserProfileId });
        if (!result.IsOk)
        {
            await NotificationService.Error("Contact not found");
            return;
        }

        contactElement.ContactExtended = result.ResponseDto;
        contactElement.NewContactName = null;
        contactElement.IsExpanded = true;
    }


    private async Task UpdateContactAsync(ContactElementModel contactElement)
    {
        if (string.IsNullOrWhiteSpace(contactElement.NewContactName)) return;

        var result = await ApiClient.AddOrUpdateContactAsync(new NewUserContactDto
        {
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