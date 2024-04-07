using Blazorise;
using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Components.Common;
using DrugSchedule.Client.Models;
using DrugSchedule.Client.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.VisualBasic;
using System.Globalization;

namespace DrugSchedule.Client.Components;

public partial class RepeatEditor
{
    [Parameter] public Func<Task<EditorModal.ModalResult>> Save { get; set; }

    [Parameter] public Func<Task<EditorModal.ModalResult>> Delete { get; set; }

    [Parameter, EditorRequired] public RepeatModel Repeat { get; set; } = default!;

    protected override void OnParametersSet()
    {
        Days = Repeat.RepeatDayOfWeek.ToArray();
        base.OnParametersSet();
    }

    private FlagEnumElement<RepeatDayOfWeekDto>[] Days { get; set; } = default!;

    public void DayOfWeekValidate(ValidatorEventArgs args)
    {
        if ((int)Repeat.RepeatDayOfWeek == 0)
        {
            args.Status = ValidationStatus.Error;
            args.ErrorText = "One ore more days must be checked";
            return;
        }

        args.Status = ValidationStatus.Success;
    }
}