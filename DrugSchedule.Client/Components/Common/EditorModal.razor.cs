using Blazorise;
using Microsoft.AspNetCore.Components;

namespace DrugSchedule.Client.Components.Common;

public partial class EditorModal
{
    public record ModalResult(bool Ok, string Message);

    private Validations _validations = default!;
    private Modal _editModal = default!;
    private CustomAlert _alert = default!;
    private const string DefaultDeleteAlert = $"Removing cant be undone. Continue?";
    private bool _isValid = true;

    [Parameter] public bool AllowToSave { get; set; } = true;

    [Inject] private INotificationService NotificationService { get; set; } = default!;

    [Parameter, EditorRequired] public RenderFragment EditorModalBody { get; set; } = default!;

    [Parameter, EditorRequired] public RenderFragment ViewerModalBody { get; set; } = default!;

    public bool IsNewItem { get; set; } = false;

    [Parameter] public bool AllowRemove { get; set; } = true;

    [Parameter] public bool AllowSave { get; set; } = true;

    [Parameter] public Func<Task<ModalResult>> Delete { get; set; } = default!;

    [Parameter] public Func<Task<ModalResult>> Save { get; set; } = default!;

    [Parameter, EditorRequired] public string ItemText { get; set; } = default!;

    [Parameter] public int EditingId { get; set; } = -1;

    private async Task ConfirmDeleteAsync()
    {
        await Hide();
        var result = await _alert.ShowYesNo(DefaultDeleteAlert, "Remove item?");
        if (result == CustomAlert.ModalResult.Yes)
        {
            var deleted = await Delete.Invoke();
            if (deleted.Ok)
            {
                await NotificationService.Success(ItemText, $"Removed");
            }
            else
            {
                await _alert.ShowOk(deleted.Message, "Error");
                await _editModal.Show();
            }
        }
        else
        {
            await Show();
        }
    }

    private async Task ConfirmSaveAsync()
    {
        bool isOk = await _validations.ValidateAll();
        await Hide();
        if (!isOk)
        {
            await NotificationService.Error("Invalid fields values", "Error");
            await Show();
            return;
        }

        var saved = await Save.Invoke();
        if (saved.Ok)
        {
            await NotificationService.Success(ItemText, $"Saved");
        }
        else
        {
            await _alert.ShowOk(saved.Message, "Error");
            await Show();
        }
    }

    private async Task OnModalClosing(ModalClosingEventArgs e)
    {
        if (e.CloseReason == CloseReason.FocusLostClosing)
        {
            e.Cancel = true;
            return;
        }
    }

    public async Task Show()
    {
        await _editModal.Show();
        _isValid = await _validations.ValidateAll();
    }

    public async Task Hide()
    {
        await _editModal.Hide();
    }
}