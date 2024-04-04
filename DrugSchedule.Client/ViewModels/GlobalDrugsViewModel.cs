using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Constants;
using DrugSchedule.Client.Networking;
using DrugSchedule.Client.Utils;
using Microsoft.AspNetCore.Components;

namespace DrugSchedule.Client.ViewModels;

public class GlobalDrugsViewModel : PageViewModelBase
{
    [SupplyParameterFromQuery(Name = "id")]
    public int MedicamentIdParameter { get; set; }

    protected MedicamentExtendedDto? Medicament { get; private set; }
 
    protected override async Task LoadAsync()
    {
        if (MedicamentIdParameter == default)
        {
            Medicament = null;
            PageState = PageState.Default;
            return;
        }

        var medicamentResult = await ApiClient.GetMedicamentExtendedAsync(new MedicamentIdDto { MedicamentId = MedicamentIdParameter });
        if (!medicamentResult.IsOk)
        {
            await ServeApiCallResult(medicamentResult);
            ToDrugsHome();
            return;
        }

        Medicament = medicamentResult.ResponseDto;
        PageState = PageState.Details;
    }

    protected void ToDrugsHome()
    {
        NavigationManager.NavigateTo(Routes.GlobalDrugs);
    }
}