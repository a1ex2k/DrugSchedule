using Blazorise;
using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Constants;
using Microsoft.AspNetCore.Components;

namespace DrugSchedule.Client.Components;

public partial class GlobalMedicamentExtended
{
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;
    [Parameter, EditorRequired] public MedicamentExtendedDto Medicament { get; set; } = default!;
    [Parameter, EditorRequired] public EventCallback<ManufacturerDto> ManufacturerSearch { get; set; }
    [Parameter, EditorRequired] public EventCallback<MedicamentReleaseFormDto> ReleaseFormSearch { get; set; }
    [Parameter] public bool Navigable { get; set; }

    public IEnumerable<DownloadableFileDto> Images => Medicament?.FileCollection.Files
        .Where(f => f.MediaType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
        ?? Enumerable.Empty<DownloadableFileDto>();

    private async Task NavigateToManufacturer()
    {
        if (!Navigable || !ManufacturerSearch.HasDelegate) return;
        await ManufacturerSearch.InvokeAsync(Medicament.Manufacturer);
    }

    private async Task NavigateToReleaseForm()
    {
        if (!Navigable || !ManufacturerSearch.HasDelegate) return;
        await ReleaseFormSearch.InvokeAsync(Medicament.ReleaseForm);
    }
}