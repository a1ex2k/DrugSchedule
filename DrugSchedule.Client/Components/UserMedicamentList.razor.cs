using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Networking;
using Microsoft.AspNetCore.Components;
using DrugSchedule.Client.Constants;

namespace DrugSchedule.Client.Components;

public partial class UserMedicamentList
{
    [Inject] public IApiClient ApiClient { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;

    [Parameter] public EventCallback<UserMedicamentSimpleDto> OnSelect { get; set; }
    [Parameter] public string SelectButtonText { get; set; } = "Select";

    [Parameter] public bool Navigable { get; set; }
    [Parameter] public bool Selectable { get; set; }

    private List<UserMedicamentSimpleDto> Medicaments { get; set; } = new();

    private string NameSearchValue { get; set; } = String.Empty;
    private string ManufacturerSearchValue { get; set; } = String.Empty;
    private string ReleaseFormSearchValue { get; set; } = String.Empty;

    protected override async Task OnInitializedAsync()
    {
        await SearchForMedicamentsAsync();
        await base.OnInitializedAsync();
    }

    private async Task SearchForMedicamentsAsync()
    {
        Medicaments = await LoadMedicamentsAsync();
    }

    private async Task NameSearchValueChanged(string value)
    {
        NameSearchValue = value;
        await SearchForMedicamentsAsync();
    }

    private async Task ReleaseFormSearchValueChanged(string value)
    {
        ReleaseFormSearchValue = value;
        await SearchForMedicamentsAsync();
    }
    private async Task ManufacturerSearchValueChanged(string value)
    {
        ManufacturerSearchValue = value;
        await SearchForMedicamentsAsync();
    }

    private async Task<List<UserMedicamentSimpleDto>> LoadMedicamentsAsync()
    {
        var filter = new UserMedicamentFilterDto
        {
            NameFilter = string.IsNullOrWhiteSpace(NameSearchValue)
                ? null
                : new StringFilterDto { StringSearchType = StringSearchDto.Contains, SubString = NameSearchValue.Trim() },
            ManufacturerNameFilter = string.IsNullOrWhiteSpace(ManufacturerSearchValue)
                ? null
                : new StringFilterDto { StringSearchType = StringSearchDto.Contains, SubString = ManufacturerSearchValue.Trim() },
            ReleaseFormNameFilter = string.IsNullOrWhiteSpace(ReleaseFormSearchValue)
                ? null
                : new StringFilterDto { StringSearchType = StringSearchDto.Contains, SubString = ReleaseFormSearchValue.Trim() },
        };

        var result = await ApiClient.GetManyUserMedicamentsAsync(filter);
        return result.IsOk ? result.ResponseDto.Medicaments : new();
    }
}