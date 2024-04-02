using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Constants;
using DrugSchedule.Client.Utils;
using Microsoft.AspNetCore.Components;

namespace DrugSchedule.Client.Components;

public partial class GlobalMedicamentListElement
{
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;
    [Parameter, EditorRequired] public MedicamentSimpleDto Medicament { get; set; } = default!;

    [Parameter] public bool Selectable { get; set; } = true;
    [Parameter] public bool Navigable { get; set; } = true;

    [Parameter] public EventCallback<MedicamentSimpleDto> OnSelect { get; set; }

    [Parameter] public string SelectButtonText { get; set; } = "Select";


    private async Task SelectAsync()
    {
        if (!Selectable || !OnSelect.HasDelegate) return;
        await OnSelect.InvokeAsync(Medicament);
    }

    private async Task Navigate()
    {
        if (!Navigable) return;
        NavigationManager.NavigateWithParameter(Routes.GlobalDrugs, "id", Medicament.Id.ToString());
    }
}