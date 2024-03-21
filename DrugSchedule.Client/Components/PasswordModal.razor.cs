using System.Text.RegularExpressions;
using Blazorise;
using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Constants;
using DrugSchedule.Client.Networking;
using Microsoft.AspNetCore.Components;

namespace DrugSchedule.Client.Components;

public partial class PasswordModal
{
    [Inject] public INotificationService NotificationService { get; set; } = default!;
    [Inject] public IApiClient ApiClient { get; set; } = default!;
    private NewPasswordDto NewPasswordDto { get; set; } = new();
    private Validations Validations { get; set; } = default!;
    private Modal EditModal { get; set; } = default!;

    public async Task Show()
    {
        NewPasswordDto = new();
        await EditModal.Show();
    }

    private void ValidateOldPassword(ValidatorEventArgs args)
    {
        args.Status = string.IsNullOrWhiteSpace(args.Value as string) ? ValidationStatus.Error : ValidationStatus.None;
        args.ErrorText = "Old password required";
    }

    protected void ValidateNewPassword(ValidatorEventArgs args)
    {
        var value = (string)args.Value;
        if (value == null || value.Length > User.MaxLength || !Regex.IsMatch(value, User.PasswordPattern))
        {
            args.Status = ValidationStatus.Error;
            args.ErrorText = User.PasswordRequirements;
            return;
        }

        args.Status = ValidationStatus.None;
    }

    private async Task ChangePasswordAsync()
    {
        if (!await Validations.ValidateAll())
        {
            await NotificationService.Error("Passwords do not meet requirements", "Error");
            return;
        }

        var result = await ApiClient.ChangePasswordAsync(NewPasswordDto);
        if (result.IsOk)
        {
            await NotificationService.Success("Password changed", "Success");
            await EditModal.Close(CloseReason.UserClosing);
            return;
        }

        await NotificationService.Error(string.Join("<br>", result.Messages), "Error");
    }
}