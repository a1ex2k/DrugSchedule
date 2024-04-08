using Blazorise;
using DrugSchedule.Client.Networking;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.JSInterop;
using DrugSchedule.Client.Utils;

namespace DrugSchedule.Client.ViewModels;

public abstract class PageViewModelBase : ComponentBase, IDisposable
{
    [Inject] protected NavigationManager NavigationManager { get; set; } = default!;
    [Inject] protected INotificationService NotificationService { get; set; } = default!;
    [Inject] protected IApiClient ApiClient { get; set; } = default!;

    protected PageState PageState { get; set; } = PageState.Default;

    private string _initialUri = default!;

    protected override async Task OnInitializedAsync()
    {
        _initialUri = NavigationManager.Uri.Split('?')[0];
        await ProcessQueryAsync();
        NavigationManager.LocationChanged += HandleLocationChanged;

        await base.OnInitializedAsync();
    }

    private async Task LoadDataAsync()
    {
        await LoadAsync();
        StateHasChanged();
    }

    protected virtual async Task LoadAsync()
    {
        await Task.CompletedTask;
    }

    private void HandleLocationChanged(object? sender, LocationChangedEventArgs args)
    {
        if(!NavigationManager.Uri.StartsWith(_initialUri)) return;
        Task.Run(ProcessQueryAsync);      
    }

    protected bool TryGetParameter<T>(string parameterName, out T value)
    {
        value = default(T)!;
        var dict = QueryHelpers.ParseQuery(NavigationManager.ToAbsoluteUri(NavigationManager.Uri).Query);
        return dict.TryGetQueryParameter(parameterName, out value);
    }

    protected virtual async Task ProcessQueryAsync()
    {
        await LoadDataAsync();
    }

    protected async Task ServeApiCallResult(ApiCallResult apiCallResult)
    {
        if (apiCallResult.IsOk) return;

        var errors = apiCallResult.NotFound?.Messages ?? apiCallResult.InvalidInput?.Messages;
        if (errors == null || errors.Count == 0)
        {
            await NotificationService.Error("Changes might not be saved", "Unexpected error");
            return;
        }

        var text = string.Join("<br>", errors);
        await NotificationService.Error(text, "Error");
    }

    public void Dispose()
    {
        NavigationManager.LocationChanged -= HandleLocationChanged;
    }
}