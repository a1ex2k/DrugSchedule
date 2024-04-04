using System.Runtime.InteropServices.JavaScript;
using Blazorise;
using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Components.Common;
using DrugSchedule.Client.Constants;
using DrugSchedule.Client.Models;
using DrugSchedule.Client.Networking;
using Microsoft.AspNetCore.Components;

namespace DrugSchedule.Client.Components;

public partial class UserMedicamentEditor
{
    [Inject] public INotificationService NotificationService { get; set; } = default!;
    [Inject] public IApiClient ApiClient { get; set; } = default!;

    [Parameter, EditorRequired] public UserMedicamentExtendedDto? ExistingMedicament { get; set; }
    [Parameter] public EventCallback<bool> AfterSave { get; set; }

    private UserMedicamentEditorModel Model { get; set; } = new();
    private EmbeddedEditor EmbeddedEditor { get; set; } = new();

    private bool Editing => ExistingMedicament != null;

    private static string ImageFilter = string.Join(", ", FileParameters.MaxUserMedicamentFileSize);
    
    protected override async Task OnParametersSetAsync()
    {
        if (ExistingMedicament == null)
        {
            Model = new();
        }
        else
        {
            Model.Name = ExistingMedicament.Name;
            Model.ReleaseForm = ExistingMedicament.ReleaseForm;
            Model.ManufacturerName = ExistingMedicament.ManufacturerName;
            Model.Description = ExistingMedicament.Description;
            Model.Composition = ExistingMedicament.Composition;
            Model.ExistingImages = ExistingMedicament.Images.Files;

            if (ExistingMedicament.BasicMedicament != null)
            {
                var basicSimple = await ApiClient.GetMedicamentAsync(new MedicamentIdDto
                    { MedicamentId = ExistingMedicament.BasicMedicament.Id });
                Model.BasicMedicament = basicSimple.ResponseDto;
            }
        }

        await base.OnParametersSetAsync();
    }


    private async Task<bool> DeleteImageAsync(DownloadableFileDto image, long userMedicamentId)
    {
        var result = await ApiClient.RemoveUserMedicamentImageAsync(new UserMedicamentImageRemoveDto
        {
            FileGuid = image.Guid,
            UserMedicamentId = userMedicamentId
        });
        
        if (result.IsOk)
        {
            ExistingMedicament?.Images.Files.RemoveAll(i => i.Guid == image.Guid);
        }
        else
        {
            await NotificationService.Success("Image not removed", "Error");
        }

        return result.IsOk;
    }


    protected async Task AddImageAsync(FileChangedEventArgs args)
    {
        foreach (var file in args.Files)
        {
            if (file is null) continue;

            if (file.Size == 0 || file.Size > FileParameters.MaxUserMedicamentFileSize)
            {
                await NotificationService.Error($"Name: {file.Name}<br>File must be up to {FileParameters.MaxUserMedicamentFileSize / 1024 / 102}] MB", "File ignored");
                continue;
            }

            if (Array.IndexOf(FileParameters.UserMedicamentFileMimeTypes, file.Type) == -1)
            {
                await NotificationService.Error($"Name: {file.Name}<br>Unsupported type", "File ignored");
                continue;
            }

            Model.NewImages.Add(file);
        }
    }

    private async Task DeleteImage(DownloadableFileDto image)
    {
        Model.DeleteImages.Add(image);
        Model.ExistingImages.Remove(image);
    }

    private async Task<bool> UploadImageAsync(IFileEntry file, long userMedicamentId)
    {
        try
        {
            await using var stream = file.OpenReadStream(FileParameters.MaxUserMedicamentFileSize);
            var uploadFile = new UploadFile
            {
                Name = file.Name,
                Stream = stream,
                ContentType = file.Type
            };

            var result = await ApiClient.AddUserMedicamentImage(new UserMedicamentIdDto { UserMedicamentId = userMedicamentId}, uploadFile);
            if (!result.IsOk)
            {
                await NotificationService.Error(string.Join("<br>", result.Messages), $"{file.Name} not uploaded");
            }

            return result.IsOk;
        }
        catch (Exception exc)
        {
        }

        return false;
    }

    protected async Task<EditorModal.ModalResult> SaveMedicamentAsync()
    {
        var saved = ExistingMedicament == null
            ? await CreateMedicamentAsync()
            : await UpdateMedicamentAsync();

        if (AfterSave.HasDelegate)
        {
            await AfterSave.InvokeAsync(true);
        }

        return new EditorModal.ModalResult(saved, []);
    }

    private async Task<bool> UpdateMedicamentAsync()
    {
        var updateDto = new UserMedicamentUpdateDto
        {
            Id = ExistingMedicament!.Id,
            BasicMedicamentId = Model.BasicMedicament?.Id,
            Name = Model.Name.Trim(),
            ReleaseForm = Model.ReleaseForm.Trim(),
            Composition = string.IsNullOrWhiteSpace(Model.Composition) ? null : Model.Composition,
            Description = string.IsNullOrWhiteSpace(Model.Description) ? null : Model.Description,
            ManufacturerName = string.IsNullOrWhiteSpace(Model.ManufacturerName) ? null : Model.ManufacturerName,
        };

        var result = await ApiClient.UpdateUserMedicamentAsync(updateDto);
        var uploadTasks = Model.NewImages.Select(f => UploadImageAsync(f, ExistingMedicament.Id));
        var deleteTasks = Model.DeleteImages.Select(f => DeleteImageAsync(f, ExistingMedicament.Id));

        await Task.WhenAll(uploadTasks);
        await Task.WhenAll(deleteTasks);

        return result.IsOk;
    }

    private async Task<bool> CreateMedicamentAsync()
    {
        var updateDto = new NewUserMedicamentDto
        {
            BasicMedicamentId = Model.BasicMedicament?.Id,
            Name = Model.Name.Trim(),
            ReleaseForm = Model.ReleaseForm.Trim(),
            Composition = string.IsNullOrWhiteSpace(Model.Composition) ? null : Model.Composition,
            Description = string.IsNullOrWhiteSpace(Model.Description) ? null : Model.Description,
            ManufacturerName = string.IsNullOrWhiteSpace(Model.ManufacturerName) ? null : Model.ManufacturerName,
        };

        var result = await ApiClient.AddUserMedicamentAsync(updateDto);

        if (result.IsOk)
        {
            var uploadTasks = Model.NewImages.Select(f => UploadImageAsync(f, result.ResponseDto.UserMedicamentId));
            await Task.WhenAll(uploadTasks);
        }
        else
        {
            await NotificationService.Error(string.Join("<br>", result.Messages), $"Medicament not added");
        }
        return result.IsOk;
    }

    private async Task<EditorModal.ModalResult> DeleteMedicamentAsync()
    {
        var result = await ApiClient.RemoveUserMedicamentAsync(new UserMedicamentIdDto
        {
            UserMedicamentId = ExistingMedicament!.Id
        });

        await AfterSave.InvokeAsync(false);
        return new EditorModal.ModalResult(result.IsOk, result.Messages);
    }

}