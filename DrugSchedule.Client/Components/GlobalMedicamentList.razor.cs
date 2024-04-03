using Blazorise.Components;
using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Networking;
using Microsoft.AspNetCore.Components;
using System;
using DrugSchedule.Client.Constants;

namespace DrugSchedule.Client.Components;

public partial class GlobalMedicamentList
{
    [Inject] public IApiClient ApiClient { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;

    [Parameter] public EventCallback<MedicamentSimpleDto> OnSelect { get; set; }
    [Parameter] public string SelectButtonText { get; set; } = "Select";

    [Parameter] public bool Navigable { get; set; }
    [Parameter] public bool Selectable { get; set; }

    private List<MedicamentSimpleDto> Medicaments { get; set; } = new();
    private List<ManufacturerDto> Manufacturers { get; set; } = new();
    private List<MedicamentReleaseFormDto> ReleaseForms { get; set; } = new();

    private string SearchValue { get; set; } = String.Empty;
    private List<int> SearchManufacturerIds { get; set; } = new();
    private List<int> SearchReleaseFormIds { get; set; } = new();


    protected override async Task OnInitializedAsync()
    {
        await SearchForMedicamentsAsync();
        await LoadReleaseFormsAsync();
        await base.OnInitializedAsync();
    }

    private async Task SearchForManufacturersAsync(AutocompleteReadDataEventArgs args)
    {
        var filter = new ManufacturerFilterDto
        {
            NameFilter = string.IsNullOrWhiteSpace(args.SearchValue)
                ? null
                : new StringFilterDto
                {
                    StringSearchType = StringSearchDto.Contains,
                    SubString = args.SearchValue.Trim()
                },
            Take = Numbers.ManufacturersLoadCount
        };

        var result = await ApiClient.GetManufacturersAsync(filter);
        if (result.IsOk)
        {
            Manufacturers = result.ResponseDto.Manufacturers;
        }
    }

    private async Task LoadReleaseFormsAsync()
    {
        var result = await ApiClient.GetReleaseFormsAsync(new MedicamentReleaseFormFilterDto());
        if (result.IsOk)
        {
            ReleaseForms = result.ResponseDto.ReleaseForms;
        }
    }

    private async Task SearchForMedicamentsAsync()
    {
        Medicaments = await LoadMedicamentsAsync();
    }

    private async Task MoreMedicamentsAsync()
    {
        var newItems = await LoadMedicamentsAsync(Medicaments.Count);
        Medicaments.AddRange(newItems);
    }


    public async Task<List<MedicamentSimpleDto>> LoadMedicamentsAsync(int skipCount = 0)
    {
        var filter = new MedicamentFilterDto
        {
            NameFilter = string.IsNullOrWhiteSpace(SearchValue)
                ? null
                : new StringFilterDto { StringSearchType = StringSearchDto.Contains, SubString = SearchValue.Trim() },
            ManufacturerFilter = SearchManufacturerIds.Count == 0
                ? null
                : new ManufacturerFilterDto
                {
                    IdFilter = SearchManufacturerIds
                },
            MedicamentReleaseFormFilter = SearchReleaseFormIds.Count == 0
                ? null
                : new MedicamentReleaseFormFilterDto
                {
                    IdFilter = SearchReleaseFormIds
                },
            Take = Numbers.MedicamentLoadCount,
            Skip = skipCount
        };

        var result = await ApiClient.GetMedicamentsAsync(filter);
        return result.IsOk ? result.ResponseDto.Medicaments : new();
    }


    private async Task ReleaseFormIdsChanged(List<int> values)
    {
        SearchReleaseFormIds = values;
        await SearchForMedicamentsAsync();
    }

    private async Task ManufacturerIdsChanged(List<int> values)
    {
        SearchManufacturerIds = values;
        await SearchForMedicamentsAsync();
    }

    private async Task SearchValueChanged(string value)
    {
        SearchValue = value;
        await SearchForMedicamentsAsync();
    }
}