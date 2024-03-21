using Blazorise;
using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Networking;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using DrugSchedule.Client.Constants;

namespace DrugSchedule.Client.ViewModels;

public class AuthViewModel : PageViewModelBase
{
    [CascadingParameter] protected Task<AuthenticationState> AuthState { get; set; } = default!;
    protected bool IsRegistration { get; set; }
    protected RegisterDto RegisterDto { get; private set; } = new();
    protected LoginDto LoginDto { get; private set; } = new();
    protected Validations RegistrationValidations { get; set; } = default!;
    protected Validations LoginValidations { get; set; } = default!;




    protected async Task LoginAsync()
    {
        if (!await LoginValidations.ValidateAll())
        {
            await NotificationService.Error(User.FormInvalid, "Error");
            return;
        }

        var result = await ApiClient.LogInAsync(LoginDto);
        await ServeApiCallResult(result);
        if (result.IsOk)
        {
            NavigationManager.NavigateTo("/");
        }

        LoginDto.Password = string.Empty;
    }

    protected async Task RegisterAsync()
    {
        if (!await RegistrationValidations.ValidateAll())
        {
            await NotificationService.Error(User.FormInvalid, "Error");
            return;
        }

        var result = await ApiClient.RegisterAsync(RegisterDto);
        await ServeApiCallResult(result);

        if (result.IsOk)
        {
            LoginDto.Username = RegisterDto.Username;
            LoginDto.Password = RegisterDto.Password;
            await LoginAsync();
            RegisterDto.Password = string.Empty;
        }
    }

    protected async Task ValidateNewUsernameAsync(ValidatorEventArgs args, CancellationToken cancellationToken)
    {
        var value = (string)args.Value;
        if (value == null || value.Length > User.MaxLength || !Regex.IsMatch(value, User.UsernamePattern))
        {
            args.Status = ValidationStatus.Error;
            args.ErrorText = User.UsernameRequirements;
            return;
        }

        var result = await ApiClient.UsernameAvailableAsync(new UsernameDto { Username = value }, cancellationToken);
        await ServeApiCallResult(result);

        if(result.ResponseDto?.IsAvailable != true)
        { 
            args.Status = ValidationStatus.Error;
            args.ErrorText = User.UsernameTaken;
        }

        args.Status = ValidationStatus.None;
    }

    protected void ValidateUsername(ValidatorEventArgs args)
    {
        var value = (string)args.Value;
        if (value == null || value.Length > User.MaxLength || !Regex.IsMatch(value, User.UsernamePattern))
        {
            args.Status = ValidationStatus.Error;
            args.ErrorText = User.UsernameRequirements;
            return;
        }

        args.Status = ValidationStatus.None;
    }

    protected void ValidatePassword(ValidatorEventArgs args)
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

    protected void ValidateEmail(ValidatorEventArgs args)
    {
        var value = (string)args.Value;
        var validator = new EmailAddressAttribute();
        if (!validator.IsValid(value))
        {
            args.Status = ValidationStatus.Error;
            args.ErrorText = User.EmailInvalid;
            return;
        }

        args.Status = ValidationStatus.None;
    }

    protected void ChangeView()
    {
        IsRegistration = !IsRegistration;
    }

    protected override async Task OnInitializedAsync()
    {
        var user = (await AuthState).User;
        if (user.Identity?.IsAuthenticated == true)
        {
            NavigationManager.NavigateTo("/");
        }
        await base.OnInitializedAsync();
    }
}