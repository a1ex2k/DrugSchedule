using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Components.Common;
using DrugSchedule.Client.Constants;
using DrugSchedule.Client.Networking;
using DrugSchedule.Client.Pages;

namespace DrugSchedule.Client.ViewModels;

public class ContactsViewModel : PageViewModelBase
{
    private long _contactIdParameter;

    protected bool IsDetailedView => _contactIdParameter != default && Contact != null;
    protected UserContactDto? Contact { get; private set; }
    protected NewUserContactDto NewUserContact { get; private set; } = new(){ UserProfileId = 0, СontactName = null };

    protected EditorModal EditorModal { get; set; } = default!;

    protected override Task ProcessQueryAsync()
    {
        TryGetParameter("id", out long value);
        return base.ProcessQueryAsync();
    }

    protected override async Task LoadAsync()
    {
        if (_contactIdParameter == default)
        {
            NewUserContact.UserProfileId = 0;
            NewUserContact.СontactName = null;
            return;
        }

        var contactResult = await ApiClient.GetSingleExtendedContactAsync(new UserIdDto { UserProfileId = _contactIdParameter });
        if (!contactResult.IsOk)
        {
            _contactIdParameter = default!;
            await ServeApiCallResult(contactResult);
            return;
        }

        Contact = contactResult.ResponseDto;
        NewUserContact = new NewUserContactDto
        {
            UserProfileId = Contact.UserProfileId,
            СontactName = Contact.Username
        };
    }

    protected async Task<EditorModal.ModalResult> UpdateContactAsync()
    {
        if (string.IsNullOrWhiteSpace(NewUserContact.СontactName))
            return new EditorModal.ModalResult(false, ["Name was empty"]);

        var result = await ApiClient.AddOrUpdateContactAsync(NewUserContact);
        if (result.IsOk && IsDetailedView)
        {
            Contact.СontactName = NewUserContact.СontactName;
        }

        var text = result.IsOk ? ["Contact saved"] : result.Messages;
        return new EditorModal.ModalResult(result.IsOk, text);
    }

    protected async Task<EditorModal.ModalResult> RemoveContactAsync()
    {
        var result = await ApiClient.RemoveContactAsync(new UserIdDto { UserProfileId = NewUserContact.UserProfileId });
        if (result.IsOk && IsDetailedView)
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