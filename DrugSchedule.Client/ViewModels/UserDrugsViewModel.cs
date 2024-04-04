using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Constants;
using DrugSchedule.Client.Networking;
using DrugSchedule.Client.Utils;

namespace DrugSchedule.Client.ViewModels;

public class UserDrugsViewModel : PageViewModelBase
{
    private long _medicamentIdParameter;
    protected UserMedicamentExtendedDto? Medicament { get; private set; }
 

    protected override async Task ProcessQueryAsync()
    {
        TryGetParameter("id", out _medicamentIdParameter);
        await base.ProcessQueryAsync();
    }

    protected override async Task LoadAsync()
    {
        if (_medicamentIdParameter == default)
        {
            Medicament = null;
            PageState = PageState.Default;
            return;
        }

        var medicamentResult = await ApiClient.GetSingleExtendedUserMedicamentAsync(new UserMedicamentIdDto { UserMedicamentId = _medicamentIdParameter });
        if (!medicamentResult.IsOk)
        {
            _medicamentIdParameter = default!;
            PageState = PageState.Default;
            await ServeApiCallResult(medicamentResult);
            return;
        }

        Medicament = medicamentResult.ResponseDto;
        PageState = PageState.Details;
    }

    protected void AfterSave(bool exist)
    {
        if (exist && PageState == PageState.Editor)
        {
            StateHasChanged();
        }
        else
        {
            ToDrugsHome();
        }
    }

    protected void ToDrugsHome()
    {
        NavigationManager.NavigateTo(Routes.GlobalDrugs);
    }

    protected void CreateNew()
    {
        PageState = PageState.New;
        StateHasChanged();
    }
}