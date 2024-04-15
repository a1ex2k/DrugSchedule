using Blazorise;
using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Components.Common;
using DrugSchedule.Client.Constants;
using DrugSchedule.Client.Networking;
using Microsoft.AspNetCore.Components;

namespace DrugSchedule.Client.Components;

public partial class ProfileEditor
{
    [Inject] public INotificationService NotificationService { get; set; } = default!;
    [Inject] public IApiClient ApiClient { get; set; } = default!;
    [Parameter, EditorRequired] public UserFullDto CurrentUser { get; set; } = default!;
    [Parameter] public EventCallback AfterSave { get; set; }
    
    [Parameter] public IFileEntry? NewAvatar { get; set; }
    private UserUpdateDto UserUpdateDto { get; set; } = new();

    private static string _avatarFilter = string.Join(", ", FileParameters.AvatarMimeTypes);
    private readonly DateTimeOffset _maxBirthDate = DateTime.Now.AddYears(-User.MinAgeYears);
    

    protected override Task OnParametersSetAsync()
    {
        UserUpdateDto.Sex = CurrentUser.Sex;
        UserUpdateDto.DateOfBirth = CurrentUser.DateOfBirth;
        UserUpdateDto.RealName = CurrentUser.RealName;
        return base.OnParametersSetAsync();
    }


    private async Task DeleteAvatarAsync()
    {
        if (CurrentUser.Avatar == null) return;

        var result = await ApiClient.RemoveAvatarAsync(new FileIdDto { FileGuid = CurrentUser.Avatar.Guid });
        if (result.IsOk)
        {
            await NotificationService.Success("Current avatar removed", "Success");
            CurrentUser.Avatar = null;
        }
        else
        {
            await NotificationService.Success("Avatar not removed", "Error");
        }

        await AfterSave.InvokeAsync();
    }

    protected async Task SelectAvatarAsync(FileChangedEventArgs args)
    {
        if (args.Files.Length == 0 || args.Files[0] == null)
        {
            NewAvatar = null;
            return;
        }

        var file = args.Files[0]!;

        if (file.Size == 0 || file.Size > FileParameters.MaxAvatarFileSize)
        {
            await NotificationService.Error($"File must be up to {FileParameters.MaxAvatarFileSize / 1024 / 102}] MB", "Avatar not changed");
            return;
        }

        if (Array.IndexOf(FileParameters.AvatarMimeTypes, file.Type) == -1)
        {
            await NotificationService.Error("File type not supported", "Avatar not changed");
            return;
        }

        NewAvatar = file;
    }

    protected async Task SetAvatarAsync()
    {
        try
        {
            await using var stream = NewAvatar!.OpenReadStream(FileParameters.MaxAvatarFileSize);
            var uploadFile = new UploadFile
            {
                Name = NewAvatar.Name,
                Stream = stream,
                ContentType = NewAvatar.Type
            };

            var result = await ApiClient.SetAvatarAsync(uploadFile);
            if (result.IsOk)
            {
                await NotificationService.Success("Avatar updated successfully", "Success");
                CurrentUser.Avatar = result.ResponseDto;
            }
            else
            {
                await NotificationService.Error(string.Join("<br>", result.Messages), "Avatar not set");
            }
        }
        catch (Exception exc)
        {
        }
        finally
        {
            await AfterSave.InvokeAsync();
        }
    }

    protected async Task<EditorModal.ModalResult> UpdateProfileAsync()
    {
        if (string.IsNullOrWhiteSpace(UserUpdateDto.RealName))
        {
            UserUpdateDto.RealName = null;
        }

        if (UserUpdateDto.DateOfBirth?.Year < 1800)
        {
            UserUpdateDto.DateOfBirth = null;
        }

        var result = await ApiClient.UpdateProfileAsync(UserUpdateDto);
        if (result.IsOk)
        {
            CurrentUser.RealName = UserUpdateDto.RealName ;
            CurrentUser.DateOfBirth = UserUpdateDto.DateOfBirth;
            CurrentUser.Sex = UserUpdateDto.Sex;
            await AfterSave.InvokeAsync();
        }

        return new EditorModal.ModalResult(result.IsOk, result.Messages);
    }
}