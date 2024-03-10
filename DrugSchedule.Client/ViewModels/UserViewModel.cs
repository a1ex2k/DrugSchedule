using Blazorise;
using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Networking;
using DrugSchedule.Client.Components;

namespace DrugSchedule.Client.ViewModels;

public class UserViewModel : PageViewModelBase
{
    protected UserFullDto CurrentUser { get; private set; } = new();
    protected List<UserContactSimpleDto> Contacts { get; private set; } = new();
    protected Validations ProfileInfoValidations { get; set; } = default!;
    protected PasswordModal PasswordModal { get; set; } = default!;


    protected override async Task LoadAsync()
    {
        var meResult = await ApiClient.GetMeAsync();
        await ServeApiCallResult(meResult);
        if (meResult.IsOk)
        {
            CurrentUser = meResult.ResponseDto;
        }

        var contactsResult = await ApiClient.GetAllContactsAsync();
        await ServeApiCallResult(contactsResult);
        if (contactsResult.IsOk)
        {
            Contacts = contactsResult.ResponseDto.Contacts;
        }
    }

    protected async Task LogOutAsync()
    {
        await ApiClient.LogoutAsync();
    }

    protected async Task OpenPasswordModalAsync()
    {
        await PasswordModal.Show();
    }

}