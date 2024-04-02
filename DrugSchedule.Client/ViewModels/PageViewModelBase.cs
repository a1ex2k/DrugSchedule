using Blazorise;
using DrugSchedule.Client.Networking;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Microsoft.JSInterop;
using DrugSchedule.Client.Utils;

namespace DrugSchedule.Client.ViewModels;

public abstract class PageViewModelBase : ComponentBase, IDisposable
{
    [Inject] protected NavigationManager NavigationManager { get; set; } = default!;
    [Inject] protected IJSRuntime JSRuntime { get; set; } = default!;
    [Inject] protected INotificationService NotificationService { get; set; } = default!;
    [Inject] protected IApiClient ApiClient { get; set; } = default!;

    protected bool NowLoading = false;
    private Dictionary<string, StringValues> _queryParameters = new();
    

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            NavigationManager.LocationChanged += HandleLocationChanged;
            await ProcessQueryAsync();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    protected async Task LoadDataAsync()
    {
        NowLoading = true;
        await InvokeAsync(StateHasChanged);
        await LoadAsync();
        NowLoading = false;
        await InvokeAsync(StateHasChanged);
    }

    protected virtual async Task LoadAsync()
    {
        await Task.CompletedTask;
    }
    
    private void HandleLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        _queryParameters = QueryHelpers.ParseQuery(NavigationManager.ToAbsoluteUri(NavigationManager.Uri).Query);
        Task.Run(async () => await ProcessQueryAsync());
    }

    protected bool TryGetParameter<T> (string parameterName, out T value)
    {
        value = default(T)!;
        return _queryParameters.TryGetQueryParameter(parameterName, out value);
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