using Blazorise;
using Microsoft.AspNetCore.Components;

namespace DrugSchedule.Client.Components.Common;

public partial class EmbeddedEditor
{
    private Validations _validations = default!;
    private CustomAlert _alert = default!;
    private const string DefaultDeleteAlert = $"Removing cant be undone. Continue?";
    private bool _isValid = true;

    [Inject] private INotificationService NotificationService { get; set; } = default!;

    [Parameter, EditorRequired] public RenderFragment EditorModalBody { get; set; } = default!;

    [Parameter] public bool AllowRemove { get; set; } = true;

    [Parameter] public EventCallback<bool> AfterSave { get; set; }

    [Parameter] public bool AllowSave { get; set; } = true;

    [Parameter] public Func<Task<EditorModal.ModalResult>> Delete { get; set; } = default!;

    [Parameter] public Func<Task<EditorModal.ModalResult>> Save { get; set; } = default!;

    [Parameter, EditorRequired] public string ItemText { get; set; } = default!;

    private async Task ConfirmDeleteAsync()
    {
        var result = await _alert.ShowYesNo(DefaultDeleteAlert, "Remove item?");
        if (result == CustomAlert.ModalResult.Yes)
        {
            var deleted = await Delete.Invoke();
            if (deleted.Ok)
            {
                await AfterSave.InvokeAsync(false);
                await NotificationService.Success(ItemText, $"Removed");
            }
            else
            {
                await _alert.ShowOk(string.Join("<br>", deleted.Messages), "Error");
            }
        }
    }

    private async Task ConfirmSaveAsync()
    {
        bool isOk = await _validations.ValidateAll();
        if (!isOk)
        {
            await NotificationService.Error("Invalid fields values", "Error");
            return;
        }

        var saved = await Save.Invoke();
        if (saved.Ok)
        {
            await NotificationService.Success(ItemText, $"Saved");
            if (AfterSave.HasDelegate)
            {
                await AfterSave.InvokeAsync(true);
            }
        }
        else
        {
            await NotificationService.Error(string.Join("<br>", saved.Messages), "Error");
        }
    }
}