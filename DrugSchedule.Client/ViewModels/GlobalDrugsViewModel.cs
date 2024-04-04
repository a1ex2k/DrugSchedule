﻿using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Constants;
using DrugSchedule.Client.Networking;
using DrugSchedule.Client.Utils;

namespace DrugSchedule.Client.ViewModels;

public class GlobalDrugsViewModel : PageViewModelBase
{
    private int _medicamentIdParameter;
    protected MedicamentExtendedDto? Medicament { get; private set; }
 

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

        var medicamentResult = await ApiClient.GetMedicamentExtendedAsync(new MedicamentIdDto { MedicamentId = _medicamentIdParameter });
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

    protected void ToDrugsHome()
    {
        NavigationManager.NavigateTo(Routes.GlobalDrugs);
    }
}