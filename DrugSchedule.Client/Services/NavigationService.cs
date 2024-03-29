using DrugSchedule.Client.Constants;
using Microsoft.AspNetCore.Components;

namespace DrugSchedule.Client.Services;

public class NavigationService
{
    private readonly NavigationManager _navigationManager;

    public NavigationService(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
    }

    public void NavigateHome()
    {
        _navigationManager.NavigateTo("/");
    }

    public void NavigateToAuth()
    {
        _navigationManager.NavigateTo(PagesRouts.Auth);
    }

    public void NavigateToSchedule()
    {
        _navigationManager.NavigateTo(PagesRouts.Schedule);
    }

    public void NavigateToContacts()
    {
        _navigationManager.NavigateTo(PagesRouts.Contacts);
    }

    public void NavigateToContact(long contactProfileId)
    {
        _navigationManager.NavigateTo($"{PagesRouts.Contacts}/{contactProfileId}");
    }
}