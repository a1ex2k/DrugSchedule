using Blazorise;
using Microsoft.AspNetCore.Components;

namespace DrugSchedule.Client.Components.Common;

public partial class EditorModal 
{
    public record ModalResult(bool Ok, IEnumerable<string> Messages);

    private Validations _validations = default!;
    private Modal _editModal = default!;
    private CustomAlert _alert = default!;
    private const string DefaultDeleteAlert = $"Removing cant be undone. Continue?";
    private bool _isValid = true;

    [Inject] private INotificationService NotificationService { get; set; } = default!;

    [Parameter, EditorRequired] public RenderFragment EditorModalBody { get; set; } = default!;

    [Parameter] public bool AllowRemove { get; set; } = true;

    [Parameter] public EventCallback AfterSave { get; set; }

    [Parameter] public bool AllowSave { get; set; } = true;

    [Parameter] public Func<Task<ModalResult>> Delete { get; set; } = default!;

    [Parameter] public Func<Task<ModalResult>> Save { get; set; } = default!;

    [Parameter, EditorRequired] public string ItemText { get; set; } = default!;

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
                await _alert.ShowOk(string.Join("<br>", deleted.Messages), "Error");
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
            await _editModal.Close(CloseReason.UserClosing);
            if (AfterSave.HasDelegate)
            {
                await AfterSave.InvokeAsync();
            }
        }
        else
        {
            await NotificationService.Error(string.Join("<br>", saved.Messages), "Error");
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