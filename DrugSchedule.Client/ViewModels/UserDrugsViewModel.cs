using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Constants;
using DrugSchedule.Client.Networking;
using DrugSchedule.Client.Utils;
using Microsoft.AspNetCore.Components;

namespace DrugSchedule.Client.ViewModels;

public class UserDrugsViewModel : PageViewModelBase
{
    [SupplyParameterFromQuery(Name = "id")]
    public long MedicamentIdParameter { get; set; }

    [SupplyParameterFromQuery(Name = "new")]
    public bool NewMedicamentParameter { get; set; }


    protected UserMedicamentExtendedDto? Medicament { get; private set; }
 

    protected override async Task LoadAsync()
    {
        if (NewMedicamentParameter)
        {
            Medicament = null;
            PageState = PageState.New;
            return;
        } 

        if (MedicamentIdParameter == default)
        {
            Medicament = null;
            PageState = PageState.Default;
            return;
        }

        var medicamentResult = await ApiClient.GetSingleExtendedUserMedicamentAsync(new UserMedicamentIdDto { UserMedicamentId = MedicamentIdParameter });
        if (!medicamentResult.IsOk)
        {
            await ServeApiCallResult(medicamentResult);
            ToDrugsHome();
            return;
        }

        Medicament = medicamentResult.ResponseDto;
        PageState = PageState.Details;
    }

    protected void AfterDelete(long id)
    {
        ToDrugsHome();
    }

    protected void AfterSave(long id)
    {
        NavigationManager.NavigateWithParameter(Routes.UserDrugs, "id", id);
    }


    protected void ToDrugsHome()
    {
        NavigationManager.NavigateTo(Routes.UserDrugs);
    }

    protected void CreateNew()
    {
        NavigationManager.NavigateWithBoolParameter(Routes.UserDrugs, "new");
    }
}