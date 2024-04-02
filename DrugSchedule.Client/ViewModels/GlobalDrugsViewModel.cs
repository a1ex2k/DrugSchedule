using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Constants;
using DrugSchedule.Client.Networking;

namespace DrugSchedule.Client.ViewModels;

public class GlobalDrugsViewModel : PageViewModelBase
{
    private int _medicamentIdParameter;

    protected bool IsDetailedView => _medicamentIdParameter != default && Medicament != null;
    protected MedicamentExtendedDto? Medicament { get; private set; }
    protected int ManufacturerId { get; private set; }
    protected int ReleaseFormId { get; private set; }
 

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
            return;
        }

        var medicamentResult = await ApiClient.GetMedicamentExtendedAsync(new MedicamentIdDto { MedicamentId = _medicamentIdParameter });
        if (!medicamentResult.IsOk)
        {
            _medicamentIdParameter = default!;
            await ServeApiCallResult(medicamentResult);
            return;
        }

        Medicament = medicamentResult.ResponseDto;
    }

    protected void ToSearchOfManufacturer()
    {
        _medicamentIdParameter = default;
        Medicament = null;
        ManufacturerId = Medicament?.Manufacturer?.Id ?? default;
        ReleaseFormId = default;
        ToDrugsHome();
    }

    protected void ToSearchOfForm()
    {
        _medicamentIdParameter = default;
        Medicament = null;
        ManufacturerId = default;
        ReleaseFormId = Medicament?.ReleaseForm.Id ?? default;
        ToDrugsHome();
    }

    protected void ToDrugsHome()
    {
        NavigationManager.NavigateTo(Routes.GlobalDrugs);
    }
}