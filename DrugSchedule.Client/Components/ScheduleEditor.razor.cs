using Blazorise;
using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Components.Common;
using DrugSchedule.Client.Models;
using DrugSchedule.Client.Networking;
using Microsoft.AspNetCore.Components;

namespace DrugSchedule.Client.Components;

public partial class ScheduleEditor
{
    [Inject] public IApiClient ApiClient { get; set; } = default!;

    [Parameter] public EventCallback<long> AfterCreate { get; set; }

    [Parameter] public EventCallback AfterDelete { get; set; }

    [Parameter] public EventCallback AfterSave { get; set; }

    [Parameter, EditorRequired] public ScheduleExtendedDto? Schedule { get; set; }

    private ScheduleModel Model { get; set; } = default!;


    protected override void OnParametersSet()
    {
        Schedule = Schedule?.ToModel() ?? new ScheduleModel();
        base.OnParametersSet();
    }


    public void MedicamentValidate(ValidatorEventArgs args)
    {
        if (Model.UserMedicament is null && Model.GlobalMedicament is null
            || Model.UserMedicament is not null && Model.GlobalMedicament is not null)
        {
            args.Status = ValidationStatus.Error;
            args.ErrorText = "Either user or global medicament must be selected";
            return;
        }

        args.Status = ValidationStatus.Success;
    }

    private async Task<EditorModal.ModalResult> CreateScheduleAsync()
    {
        var newDto = new NewScheduleDto
        {
            GlobalMedicamentId = Model.GlobalMedicament?.Id,
            UserMedicamentId = Model.UserMedicament?.Id,
            Information = Model.Information?.Trim(),
            Enabled = true,
            Shares = Model.Shares?
                .Where(x => x.Contact != null)
                .Select(x => new NewScheduleSharePartDto
                {
                    CommonContactProfileId = x.Contact!.UserProfileId,
                    Comment = x.Comment?.Trim(),
                }).ToList(),
        };

        var result = await ApiClient.
        return !result.IsOk ? null : result.ResponseDto.UserMedicamentId;
    }

}