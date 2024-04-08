using Blazorise;
using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Components.Common;
using DrugSchedule.Client.Models;
using DrugSchedule.Client.Networking;
using DrugSchedule.Client.Utils;
using Microsoft.AspNetCore.Components;

namespace DrugSchedule.Client.Components;

public partial class RepeatEditor
{
    [Inject] public IApiClient ApiClient { get; set; } = default!;

    [Parameter] public EventCallback<long> AfterDelete { get; set; }

    [Parameter] public EventCallback<long> AfterSave { get; set; }

    [Parameter] public bool Removable { get; set; }

    [Parameter, EditorRequired] public ScheduleRepeatDto? Repeat { get; set; } = default!;

    [Parameter, EditorRequired] public long ScheduleId { get; set; } = default!;

    [Parameter, EditorRequired] public long TempId { get; set; } = default!;

    public RepeatModel Model { get; set; } = new()!;

    protected override void OnParametersSet()
    {
        Model = Repeat?.ToModel() ?? new();
        Model.ScheduleId = ScheduleId;
    }


    public void DayOfWeekValidate(ValidatorEventArgs args)
    {
        if (Model.Days.All(d => !d.Checked))
        {
            args.Status = ValidationStatus.Error;
            args.ErrorText = "One ore more days must be checked";
            return;
        }

        args.Status = ValidationStatus.None;
    }

    public void TimeValidate(ValidatorEventArgs args)
    {
        if (Model.TimeOfDay == TimeOfDayDto.None)
        {
            args.Status = ValidationStatus.Error;
            args.ErrorText = "Specify time";
            return;
        }

        args.Status = ValidationStatus.None;
    }

    protected async Task<EditorModal.ModalResult> SaveRepeatAsync()
    {
        if (Model.IsNew)
        {
            return await CreateRepeatAsync();
        }

        return await UpdateRepeatAsync();
    }


    private async Task<EditorModal.ModalResult> UpdateRepeatAsync()
    {
        var updateDto = new ScheduleRepeatUpdateDto
        {
            Id = Model.RepeatId,
            BeginDate = Model.BeginDate,
            Time = Model.Time,
            TimeOfDay = Model.TimeOfDay,
            RepeatDayOfWeek = Model.Days.ToEnum(),
            EndDate = Model.EndDate,
            TakingRule = Model.TakingRule?.Trim()
        };

        var result = await ApiClient.UpdateRepeatAsync(updateDto);
        if (result.IsOk)
        {
            await AfterSave.InvokeAsync(TempId);
        }

        return new EditorModal.ModalResult(result.IsOk, result.Messages);
    }

    private async Task<EditorModal.ModalResult> CreateRepeatAsync()
    {
        var updateDto = new NewScheduleRepeatDto
        {
            ScheduleId = Model.ScheduleId,
            BeginDate = Model.BeginDate,
            Time = Model.Time,
            TimeOfDay = Model.TimeOfDay,
            RepeatDayOfWeek = Model.Days.ToEnum(),
            EndDate = Model.EndDate,
            TakingRule = Model.TakingRule?.Trim()
        };

        var result = await ApiClient.CreateRepeatAsync(updateDto);
        if (result.IsOk)
        {
            Model.RepeatId = result.ResponseDto.RepeatId;
            await AfterSave.InvokeAsync(TempId);
        }

        return new EditorModal.ModalResult(result.IsOk, result.Messages);
    }


    private async Task<EditorModal.ModalResult> DeleteRepeatAsync()
    {
        if (Model.IsNew)
        {
            await AfterDelete.InvokeAsync(TempId);
            new EditorModal.ModalResult(true, []);
        }
        var result = await ApiClient.RemoveRepeatAsync(new RepeatIdDto
        {
            RepeatId = Model.RepeatId
        });

        if (result.IsOk)
        {
            await AfterDelete.InvokeAsync(Model.RepeatId);
        }

        return new EditorModal.ModalResult(result.IsOk, result.Messages);
    }
}