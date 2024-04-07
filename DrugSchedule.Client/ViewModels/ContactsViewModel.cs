using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Components;
using DrugSchedule.Client.Components.Common;
using DrugSchedule.Client.Constants;
using DrugSchedule.Client.Models;
using DrugSchedule.Client.Networking;
using DrugSchedule.Client.Pages;
using DrugSchedule.Client.Utils;
using Microsoft.AspNetCore.Components;

namespace DrugSchedule.Client.ViewModels;

public class ContactsViewModel : PageViewModelBase
{
    [SupplyParameterFromQuery(Name = "id")]
    private long ContactUserIdParameter { get; set; }

    protected UserContactDto? Contact { get; private set; }
    protected ContactEditorModel ContactModel { get; private set; } = new();

    protected EditorModal EditorModal { get; set; } = default!;
    protected ContactsList ContactsList { get; set; } = default!;


    protected override async Task LoadAsync()
    {
        if (ContactUserIdParameter == default)
        {
            PageState = PageState.Default;
            return;
        }

        var contactResult = await ApiClient.GetSingleExtendedContactAsync(new UserIdDto { UserProfileId = ContactUserIdParameter });
        if (!contactResult.IsOk)
        {
            await ServeApiCallResult(contactResult);
            ToContactsHome();
            return;
        }

        Contact = contactResult.ResponseDto;
        ContactModel = new ContactEditorModel
        {
            Id = Contact.UserProfileId,
            ContactName = Contact.СontactName,
        };
        PageState = PageState.Details;
    }

    protected async Task<EditorModal.ModalResult> UpdateContactAsync()
    {
        var dto = new NewUserContactDto { UserProfileId = ContactModel.Id, СontactName = ContactModel.ContactName?.Trim()! };
        var result = await ApiClient.AddOrUpdateContactAsync(dto);
        if (result.IsOk)
        {
            Contact.СontactName = ContactModel.ContactName!;
            if (PageState == PageState.Default)
            {
                await ContactsList.LoadContactsAsync();
            }

            StateHasChanged();
        }

        var text = result.IsOk ? ["Contact saved"] : result.Messages;
        return new EditorModal.ModalResult(result.IsOk, text);
    }

    protected async Task<EditorModal.ModalResult> RemoveContactAsync()
    {
        var result = await ApiClient.RemoveContactAsync(new UserIdDto { UserProfileId = ContactModel.Id });
        if (result.IsOk)
        {
            ToContactsHome();
        }

        var text = result.IsOk ? ["Contact removed"] : result.Messages;
        return new EditorModal.ModalResult(result.IsOk, text);
    }

    protected async Task OnUserSelectAsync(PublicUserDto user)
    {
        ContactModel = new ContactEditorModel
        {
            Id = user.Id,
            RealName = user.RealName,
            UserName = user.Username,
            ContactName = user.RealName ?? user.Username,
        };

        await ShowEditorAsync();
    }

    protected async Task OnContactSelectAsync(UserContactSimpleDto contact)
    {
        NavigationManager.NavigateWithParameter(Routes.Contacts, "id", contact.UserProfileId);
    }

    protected async Task ShowEditorAsync()
    {
        await EditorModal.Show();
    }

    protected void ToContactsHome()
    {
        NavigationManager.NavigateTo(Routes.Contacts);
    }
}