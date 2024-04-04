using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Networking;
using DrugSchedule.Client.Components;
using DrugSchedule.Client.Constants;
using DrugSchedule.Client.Utils;

namespace DrugSchedule.Client.ViewModels;

public class ProfileViewModel : PageViewModelBase
{
    protected UserFullDto CurrentUser { get; private set; } = new();
    protected PasswordModal PasswordModal { get; set; } = default!;


    protected override async Task LoadAsync()
    {
        var meResult = await ApiClient.GetMeAsync();
        await ServeApiCallResult(meResult);
        if (meResult.IsOk)
        {
            CurrentUser = meResult.ResponseDto;
        }
    }

    protected async Task LogOutAsync()
    {
        await ApiClient.LogoutAsync();
        NavigationManager.NavigateTo(Routes.Auth);
    }

    protected async Task OpenPasswordModalAsync()
    {
        await PasswordModal.Show();
    }
}