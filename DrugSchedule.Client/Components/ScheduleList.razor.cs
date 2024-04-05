using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Networking;
using Microsoft.AspNetCore.Components;
using DrugSchedule.Client.Constants;

namespace DrugSchedule.Client.Components;

public partial class ScheduleList
{
    [Inject] public IApiClient ApiClient { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;

    [Parameter] public EventCallback<ScheduleSimpleDto> OnSelect { get; set; }
    [Parameter] public string SelectButtonText { get; set; } = "Select";

    [Parameter] public bool Navigable { get; set; }
    [Parameter] public bool Selectable { get; set; }

    private List<ScheduleSimpleDto> Schedules { get; set; } = new();
    private List<ScheduleSimpleDto> _allSchedules = new();

    private string SearchValue { get; set; } = String.Empty;
    private bool EnabledOnly { get; set; }


    protected override async Task OnInitializedAsync()
    {
        await LoadSchedulesAsync();
        ApplyLocalFilter();
        await base.OnInitializedAsync();
    }
    
    private void SearchValueChanged(string value)
    {
        SearchValue = value;
        ApplyLocalFilter();
    }

    private void EnabledValueChanged(bool value)
    {
        EnabledOnly = value;
        ApplyLocalFilter();
    }

    private void ApplyLocalFilter()
    {
        if (!string.IsNullOrWhiteSpace(SearchValue) && !EnabledOnly)
        {
            Schedules = _allSchedules;
            return;
        }

        IEnumerable<ScheduleSimpleDto> filtered = _allSchedules;

        if (!string.IsNullOrWhiteSpace(SearchValue))
        {
            var str = SearchValue.Trim();
            filtered = filtered
                .Where(x => x.MedicamentName!.Contains(str, StringComparison.InvariantCultureIgnoreCase) ||
                            x.MedicamentReleaseFormName!.Contains(str, StringComparison.InvariantCultureIgnoreCase));
        }

        if (EnabledOnly)
        {
            filtered = filtered
                .Where(x => x.Enabled);
        }

        Schedules = filtered.ToList();
    }

    private async Task LoadSchedulesAsync()
    {
        var filter = new TakingScheduleFilterDto
        {
            OwnedOnly = true
        };

        var result = await ApiClient.GetSchedulesSimpleAsync(filter);
        if (result != null)
        {
            _allSchedules = result.ResponseDto.Schedules;
        }
    }
}