using Blazorise.Components;
using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Networking;
using Microsoft.AspNetCore.Components;
using System;

namespace DrugSchedule.Client.Components;

public partial class GlobalMedicamentList
{
    [Inject] public IApiClient ApiClient { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;

    [Parameter] public EventCallback<MedicamentSimpleDto> OnSelect { get; set; }
    [Parameter] public string SelectButtonText { get; set; } = "Select";

    [Parameter] public bool Navigable { get; set; }
    [Parameter] public bool Selectable { get; set; }
    [Parameter] public int ReleaseFormId { get; set; }
    [Parameter] public int ManufacturerId { get; set; }

    private List<MedicamentSimpleDto> Medicaments { get; set; } = new();
    private List<ManufacturerDto> Manufacturers { get; set; } = new();
    private List<MedicamentReleaseFormDto> ReleaseForms { get; set; } = new();

    private string SearchValue { get; set; } = String.Empty;
    private List<int> SearchManufacturers { get; set; } = new();
    private List<int> SearchReleaseForms { get; set; } = new();


    protected override async Task OnInitializedAsync()
    {
        await SearchForMedicamentsAsync();
        await base.OnInitializedAsync();
    }

    private async Task SearchManufacturersAsync(AutocompleteReadDataEventArgs args)
    {
        var filter = new ManufacturerFilterDto
        {
            IdFilter = ManufacturerId == default ? null : [ManufacturerId],
            NameFilter = string.IsNullOrWhiteSpace(args.SearchValue)
                ? null
                : new StringFilterDto
                {
                    StringSearchType = StringSearchDto.Contains,
                    SubString = args.SearchValue.Trim()
                },
            Take = 30
        };

        var result = await ApiClient.GetManufacturersAsync(filter);
        if (result.IsOk)
        {
            Manufacturers = result.ResponseDto.Manufacturers;
        }
    }

    private async Task SearchReleaseFormsAsync(AutocompleteReadDataEventArgs args)
    {
        var filter = new MedicamentReleaseFormFilterDto()
        {
            IdFilter = ManufacturerId == default ? null : [ReleaseFormId],
            NameFilter = string.IsNullOrWhiteSpace(args.SearchValue)
                ? null
                : new StringFilterDto
                {
                    StringSearchType = StringSearchDto.Contains,
                    SubString = args.SearchValue.Trim()
                },
            Take = 30
        };

        var result = await ApiClient.GetReleaseFormsAsync(filter);
        if (result.IsOk)
        {
            ReleaseForms = result.ResponseDto.ReleaseForms;
        }
    }

    public async Task SearchForMedicamentsAsync()
    {
        var filter = new MedicamentFilterDto
        {
            NameFilter = string.IsNullOrWhiteSpace(SearchValue)
                ? null
                : new StringFilterDto { StringSearchType = StringSearchDto.Contains, SubString = SearchValue.Trim() },
            ManufacturerFilter = SearchManufacturers.Count == 0
                ? null
                : new ManufacturerFilterDto
                {
                    IdFilter = SearchManufacturers
                },
            MedicamentReleaseFormFilter = SearchManufacturers.Count == 0
                ? null
                : new MedicamentReleaseFormFilterDto
                {
                    IdFilter = SearchReleaseForms
                },
            Take = 30
        };

        var result = await ApiClient.GetMedicamentsAsync(filter);
        if (result.IsOk)
        {
            Medicaments = result.ResponseDto.Medicaments;
        }
    }


    private async Task SearchValueChanged(string value)
    {
        SearchValue = value;
        await SearchForMedicamentsAsync();
    }
}