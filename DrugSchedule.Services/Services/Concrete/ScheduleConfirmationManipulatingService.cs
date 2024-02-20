using DrugSchedule.Services.Converters;
using DrugSchedule.Services.Models;
using DrugSchedule.Services.Services.Abstractions;
using DrugSchedule.Services.Utils;
using DrugSchedule.StorageContract.Abstractions;
using DrugSchedule.StorageContract.Data;
using OneOf.Types;

namespace DrugSchedule.Services.Services;

public class ScheduleConfirmationManipulatingService : IScheduleConfirmationManipulatingService
{
    private readonly IScheduleRepeatRepository _repeatRepository;
    private readonly IScheduleConfirmationRepository _confirmationRepository;
    private readonly ICurrentUserIdentifier _currentUserIdentifier;
    private readonly IFileService _fileService;
    private readonly IDownloadableFileConverter _downloadableFileConverter;

    public ScheduleConfirmationManipulatingService(IScheduleRepeatRepository repeatRepository, IScheduleConfirmationRepository confirmationRepository, ICurrentUserIdentifier currentUserIdentifier, IDownloadableFileConverter downloadableFileConverter, IFileService fileService)
    {
        _repeatRepository = repeatRepository;
        _confirmationRepository = confirmationRepository;
        _currentUserIdentifier = currentUserIdentifier;
        _downloadableFileConverter = downloadableFileConverter;
        _fileService = fileService;
    }

    public async Task<OneOf<ConfirmationId, NotFound, InvalidInput>> CreateConfirmationAsync(NewTakingСonfirmation newConfirmation, CancellationToken cancellationToken = default)
    {
        var repeat = await _repeatRepository.GetRepeatAsync(newConfirmation.RepeatId, _currentUserIdentifier.UserId, cancellationToken);
        if (repeat == null)
        {
            return new NotFound(ErrorMessages.UserDoesntHaveRepeat);
        }

        var matchSchedule = DoesRepeatParametersMatch(repeat, newConfirmation.ForDate, newConfirmation.ForTime,
            newConfirmation.ForTimeOfDay);
        if (!matchSchedule)
        {
            return new InvalidInput(ErrorMessages.ConfirmationNotMeetTimetable);
        }

        var confirmation = new TakingСonfirmationPlain
        {
            CreatedAt = DateTime.UtcNow,
            Text = newConfirmation.Text?.Trim().Limit(),
            ScheduleRepeatId = repeat.Id,
            ForDate = newConfirmation.ForDate,
            ForTime = newConfirmation.ForTime,
            ForTimeOfDay = newConfirmation.ForTimeOfDay
        };

        var saved = await _confirmationRepository.CreateConfirmationAsync(confirmation, cancellationToken);
        return saved != null
            ? new ConfirmationId(saved.Id, saved.ScheduleRepeatId)
            : new NotFound(ErrorMessages.UserDoesntHaveRepeat);
    }

    public async Task<OneOf<ConfirmationId, NotFound, InvalidInput>> UpdateConfirmationAsync(TakingСonfirmationUpdate update, CancellationToken cancellationToken = default)
    {
        var repeatExists = await _confirmationRepository.DoesConfirmationExistAsync(update.Id, update.RepeatId, _currentUserIdentifier.UserId, cancellationToken);
        if (!repeatExists)
        {
            return new NotFound(ErrorMessages.UserDoesntHaveConfirmation);
        }

        var updateFlags = new TakingСonfirmationUpdateFlags { Text = true };
        var confirmation = new TakingСonfirmationPlain
        {
            ScheduleRepeatId = update.RepeatId,
            Id = update.RepeatId,
            Text = update.Text?.Trim().Limit()
        };

        var saved = await _confirmationRepository.UpdateConfirmationAsync(confirmation, updateFlags, cancellationToken);
        return saved != null
            ? new ConfirmationId(saved.Id, saved.ScheduleRepeatId)
            : new NotFound(ErrorMessages.UserDoesntHaveConfirmation);
    }

    public async Task<OneOf<True, NotFound>> RemoveConfirmationAsync(ConfirmationId confirmationId, CancellationToken cancellationToken = default)
    {
        var exists = await _confirmationRepository.DoesConfirmationExistAsync(confirmationId.Id,
            confirmationId.RepeatId, _currentUserIdentifier.UserId, cancellationToken);

        if (!exists)
        {
            return new NotFound(ErrorMessages.UserDoesntHaveConfirmation);
        }

        var removeResult =
            await _confirmationRepository.RemoveConfirmationAsync(confirmationId.Id, cancellationToken);
        return removeResult == RemoveOperationResult.Removed
            ? new True()
            : new NotFound(ErrorMessages.UserDoesntHaveConfirmation);
    }

    public async Task<OneOf<DownloadableFile, NotFound, InvalidInput>> AddConfirmationImageAsync(ConfirmationId confirmationId, InputFile inputFile,
        CancellationToken cancellationToken = default)
    {
        var exists = await _confirmationRepository.DoesConfirmationExistAsync(confirmationId.Id,
            confirmationId.RepeatId, _currentUserIdentifier.UserId, cancellationToken);

        if (!exists)
        {
            return new NotFound(ErrorMessages.UserDoesntHaveConfirmation);
        }

        var addResult = await _fileService.CreateAsync(inputFile, FileCategory.DrugConfirmation.GetAwaitableParams(), FileCategory.DrugConfirmation, cancellationToken);
        if (addResult.IsT1) return addResult.AsT1;

        _ = await _confirmationRepository.AddConfirmationImageAsync(confirmationId.Id, addResult.AsT0.Guid, cancellationToken);
        return _downloadableFileConverter.ToFileModel(addResult.AsT0, true)!;
    }

    public async Task<OneOf<True, NotFound>> RemoveConfirmationImageAsync(ConfirmationId confirmationId, Guid fileGuid,
        CancellationToken cancellationToken = default)
    {
        var exists = await _confirmationRepository.DoesConfirmationExistAsync(confirmationId.Id,
            confirmationId.RepeatId, _currentUserIdentifier.UserId, cancellationToken);

        if (!exists)
        {
            return new NotFound(ErrorMessages.UserDoesntHaveConfirmation);
        }

        _ = await _confirmationRepository.RemoveConfirmationImageAsync(confirmationId.Id, fileGuid, cancellationToken);
        return new True();
    }

    private bool DoesRepeatParametersMatch(ScheduleRepeatPlain repeat, DateOnly date, TimeOnly? time,
        TimeOfDay timeOfDay)
    {
        if (time != repeat.Time) return false;
        if (timeOfDay != repeat.TimeOfDay) return false;
        if (date < repeat.BeginDate || date > repeat.EndDate ||
            ((byte)date.DayOfWeek & (byte)repeat.RepeatDayOfWeek) == 0)
        {
            return false;
        }

        return true;
    }
}