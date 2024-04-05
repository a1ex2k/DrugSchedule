using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Components;
using DrugSchedule.Client.Components.Common;
using DrugSchedule.Client.Constants;
using DrugSchedule.Client.Networking;
using DrugSchedule.Client.Utils;
using Microsoft.AspNetCore.Components;

namespace DrugSchedule.Client.ViewModels;

public class ContactsViewModel : PageViewModelBase
{
    [SupplyParameterFromQuery(Name = "id")]
    private long ContactUserIdParameter { get; set; }

    protected UserContactDto? Contact { get; private set; }
    protected NewUserContactDto NewUserContact { get; private set; } = new(){ UserProfileId = 0, СontactName = null };

    protected EditorModal EditorModal { get; set; } = default!;
    protected ContactsList ContactsList { get; set; } = default!;


    protected override async Task LoadAsync()
    {
        if (ContactUserIdParameter == default)
        {
            NewUserContact.UserProfileId = 0;
            NewUserContact.СontactName = null;
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
        NewUserContact = new NewUserContactDto
        {
            UserProfileId = Contact.UserProfileId,
            СontactName = Contact.Username
        };
        PageState = PageState.Details;
    }

    protected async Task<EditorModal.ModalResult> UpdateContactAsync()
    {
        var result = await ApiClient.AddOrUpdateContactAsync(NewUserContact);
        if (result.IsOk)
        {
            Contact.СontactName = NewUserContact.СontactName;
            if (PageState == PageState.Default)
            {
                await ContactsList.LoadContactsAsync();
            }
        }

        var text = result.IsOk ? ["Contact saved"] : result.Messages;
        return new EditorModal.ModalResult(result.IsOk, text);
    }

    protected async Task<EditorModal.ModalResult> RemoveContactAsync()
    {
        var result = await ApiClient.RemoveContactAsync(new UserIdDto { UserProfileId = NewUserContact.UserProfileId });
        if (result.IsOk)
        {
            ToContactsHome();
        }

        var text = result.IsOk ? ["Contact removed"] : result.Messages;
        return new EditorModal.ModalResult(result.IsOk, text);
    }

    protected async Task OnUserSelectAsync(PublicUserDto user)
    {
        NewUserContact = new NewUserContactDto
        {
            UserProfileId = user.Id,
            СontactName = null
        };

        await EditorModal.Show();
    }

    protected async Task OnContactSelectAsync(UserContactSimpleDto contact)
    {
        NewUserContact = new NewUserContactDto
        {
            UserProfileId = contact.UserProfileId,
            СontactName = contact.СontactName
        };

        NavigationManager.NavigateWithParameter(Routes.Contacts, "id", contact.UserProfileId.ToString());
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