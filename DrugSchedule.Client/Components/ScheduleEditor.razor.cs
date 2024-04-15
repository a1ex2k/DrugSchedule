using Blazorise;
using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Components.Common;
using DrugSchedule.Client.Models;
using DrugSchedule.Client.Networking;
using DrugSchedule.Client.Utils;
using Microsoft.AspNetCore.Components;

namespace DrugSchedule.Client.Components;

public partial class ScheduleEditor
{
    [Inject] public IApiClient ApiClient { get; set; } = default!;

    [Parameter] public EventCallback<long> AfterCreate { get; set; }

    [Parameter] public EventCallback<long> AfterDelete { get; set; }

    [Parameter] public EventCallback<long> AfterSave { get; set; }

    [Parameter, EditorRequired] public ScheduleExtendedDto? Schedule { get; set; }

    private ScheduleModel Model { get; set; } = default!;

    private long _nextRepeatTempId = -1;


    protected override void OnParametersSet()
    {
        Model = Schedule?.ToModel() ?? new ScheduleModel();
        base.OnParametersSet();
    }

    private void MedicamentValidate(ValidatorEventArgs args)
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

    private void AddNewRepeat()
    {
        Model.Repeats.Add(new KeyValuePair<long, ScheduleRepeatDto>(_nextRepeatTempId--, null));
    }

    private void RemoveMedicament()
    {
        Model.GlobalMedicament = null;
        Model.UserMedicament = null;
    } 
    
    private void SelectGlobalMedicament(MedicamentSimpleDto medicament)
    {
        Model.GlobalMedicament = medicament;
        Model.UserMedicament = null;
    }

    private void SelectUserMedicament(UserMedicamentSimpleDto medicament)
    {
        Model.GlobalMedicament = null;
        Model.UserMedicament = medicament;
    }

    private void RemoveRepeat(long key)
    {
        Model.Repeats.RemoveAll(x => x.Key == key);
    }

    private async Task<EditorModal.ModalResult> SaveScheduleAsync()
    {
        if (Model.IsNew)
        {
            return await CreateScheduleAsync();
        }

        return await UpdateScheduleAsync();
    }


    private async Task<EditorModal.ModalResult> UpdateScheduleAsync()
    {
        var updateDto = new ScheduleUpdateDto
        {
            Id = Model.ScheduleId,
            GlobalMedicamentId = Model.GlobalMedicament?.Id,
            UserMedicamentId = Model.UserMedicament?.Id,
            Information = Model.Information?.Trim(),
            Enabled = Model.Enabled,
        };

        var result = await ApiClient.UpdateScheduleAsync(updateDto);
        if (result.IsOk)
        {
            foreach (var share in Model.NewShares.Where(x => x.Contact != null))
            {
                await ApiClient.AddOrUpdateShareAsync(new ScheduleShareUpdateDto
                {
                    ScheduleId = Model.ScheduleId,
                    CommonContactProfileId = share.Contact!.UserProfileId,
                    Comment = share.Comment
                });
            }
            foreach (var share in Model.DeleteShares)
            {
                await ApiClient.RemoveShareAsync(new ScheduleShareRemoveDto
                {
                    ScheduleId = Model.ScheduleId,
                    CommonContactProfileId = share.UserContact.UserProfileId,
                });
            }
            await AfterSave.InvokeAsync(Model.ScheduleId);
        }

        return new EditorModal.ModalResult(result.IsOk, result.Messages);
    }

    private async Task<EditorModal.ModalResult> CreateScheduleAsync()
    {
        var newDto = new NewScheduleDto
        {
            GlobalMedicamentId = Model.GlobalMedicament?.Id,
            UserMedicamentId = Model.UserMedicament?.Id,
            Information = Model.Information?.Trim(),
            Enabled = true,
            Shares = Model.NewShares?
                .Where(x => x.Contact != null)
                .Select(x => new NewScheduleSharePartDto
                {
                    CommonContactProfileId = x.Contact!.UserProfileId,
                    Comment = x.Comment?.Trim(),
                }).ToList(),
        };

        var result = await ApiClient.CreateScheduleAsync(newDto);
        if (result.IsOk)
        {
            Model.ScheduleId = result.ResponseDto.ScheduleId;
            await AfterCreate.InvokeAsync(Model.ScheduleId);
        }

        StateHasChanged();
        return new EditorModal.ModalResult(result.IsOk, result.Messages);
    }


    private async Task<EditorModal.ModalResult> DeleteScheduleAsync()
    {
        if (Model.IsNew)
        {
            await AfterDelete.InvokeAsync();
            return new EditorModal.ModalResult(true, []);
        }

        var result = await ApiClient.RemoveScheduleAsync(new ScheduleIdDto
        {
            ScheduleId = Model.ScheduleId
        });

        if (result.IsOk)
        {
            await AfterDelete.InvokeAsync(Model.ScheduleId);
        }

        return new EditorModal.ModalResult(result.IsOk, result.Messages);
    }
}