using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Networking;
using Microsoft.AspNetCore.Components;

namespace DrugSchedule.Client.Components;

public partial class ContactsList
{
    [Inject] public IApiClient ApiClient { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;

    [Parameter] public EventCallback<UserContactSimpleDto> OnSelect { get; set; }
    [Parameter] public string SelectButtonText { get; set; } = "Select";

    [Parameter] public bool Navigable { get; set; }
    [Parameter] public bool Selectable { get; set; }
    [Parameter] public bool CommonOnly { get; set; }

    private List<UserContactSimpleDto> Contacts { get; set; } = new();
    private string SearchValue { get; set; } = String.Empty;
    private bool Common { get; set; }

    private List<UserContactSimpleDto>? _contactListInternal;


    protected override async Task OnInitializedAsync()
    {
        await LoadContactsAsync();
        await base.OnInitializedAsync();
    }


    public async Task LoadContactsAsync()
    {
        var result = CommonOnly ? await ApiClient.GetCommonContactsAsync() : await ApiClient.GetAllContactsAsync();
        if (result.IsOk)
        {
            _contactListInternal = result.ResponseDto.Contacts;
        }

        ApplyLocalFilter();
    }


    private void SearchValueChanged(string value)
    {
        SearchValue = value;
        ApplyLocalFilter();
    }

    private void CommonChanged(bool value)
    {
        Common = CommonOnly || value;
        ApplyLocalFilter();
    }


    private void ApplyLocalFilter()
    {
        if (string.IsNullOrWhiteSpace(SearchValue))
        {
            Contacts = _contactListInternal ?? new();
            return;
        }

        Contacts = _contactListInternal?.Where(c =>
                c.СontactName.Contains(SearchValue, StringComparison.InvariantCultureIgnoreCase)
                && c.IsCommon == (CommonOnly || Common))
            .ToList() ?? new();
    }
}