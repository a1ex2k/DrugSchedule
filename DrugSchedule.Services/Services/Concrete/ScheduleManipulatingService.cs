using DrugSchedule.Services.Models;
using DrugSchedule.Services.Services.Abstractions;
using DrugSchedule.StorageContract.Abstractions;
using DrugSchedule.StorageContract.Data;
using OneOf.Types;

namespace DrugSchedule.Services.Services;

public class ScheduleManipulatingService : IScheduleManipulatingService
{
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IScheduleRepeatRepository _repeatRepository;
    private readonly IScheduleConfirmationRepository _confirmationRepository;
    private readonly IScheduleShareRepository _shareRepository;
    private readonly ICurrentUserIdentifier _currentUserIdentifier;
    private readonly IUserContactRepository _contactRepository;
    private readonly IReadonlyDrugRepository _drugRepository;
    private readonly IUserDrugRepository _userDrugRepository;

    public ScheduleManipulatingService(IScheduleRepository scheduleRepository,
        IScheduleRepeatRepository repeatRepository, IScheduleConfirmationRepository confirmationRepository,
        IScheduleShareRepository shareRepository, ICurrentUserIdentifier currentUserIdentifier,
        IUserContactRepository contactRepository, IReadonlyDrugRepository drugRepository,
        IUserDrugRepository userDrugRepository)
    {
        _scheduleRepository = scheduleRepository;
        _repeatRepository = repeatRepository;
        _confirmationRepository = confirmationRepository;
        _shareRepository = shareRepository;
        _currentUserIdentifier = currentUserIdentifier;
        _contactRepository = contactRepository;
        _drugRepository = drugRepository;
        _userDrugRepository = userDrugRepository;
    }

    public async Task<OneOf<ScheduleId, NotFound, InvalidInput>> CreateScheduleAsync(NewSchedule newSchedule,
        CancellationToken cancellationToken = default)
    {
        var error = await GetScheduleParametersErrorsAsync(
            null, newSchedule.UserMedicamentId, newSchedule.GlobalMedicamentId, cancellationToken);
        if (error != null)
        {
            return error.Value.IsT0 ? error.Value.AsT0 : error.Value.AsT1;
        };

        var schedule = new TakingSchedulePlain
        {
            UserProfileId = _currentUserIdentifier.UserId,
            GlobalMedicamentId = newSchedule.GlobalMedicamentId,
            UserMedicamentId = newSchedule.UserMedicamentId,
            Information = newSchedule.Information?.Trim(),
            CreatedAt = DateTime.UtcNow,
            Enabled = newSchedule.Enabled
        };

        var saved = await _scheduleRepository.CreateTakingScheduleAsync(schedule, cancellationToken);
        return (ScheduleId)(saved?.Id ?? 0);
    }

    public async Task<OneOf<ScheduleId, NotFound, InvalidInput>> UpdateScheduleAsync(ScheduleUpdate update,
        CancellationToken cancellationToken = default)
    {
        var error = await GetScheduleParametersErrorsAsync(
            update.Id, update.UserMedicamentId, update.GlobalMedicamentId, cancellationToken);
        if (error != null)
        {
            return error.Value.IsT0 ? error.Value.AsT0 : error.Value.AsT1;
        };

        var schedule = new TakingSchedulePlain
        {
            Id = update.Id,
            GlobalMedicamentId = update.GlobalMedicamentId,
            UserMedicamentId = update.UserMedicamentId,
            Information = update.Information?.Trim(),
            Enabled = update.Enabled
        };

        var updateFlags = new TakingScheduleUpdateFlags
        {
            GlobalMedicamentId = true,
            UserMedicamentId = true,
            Information = true,
            Enabled = true
        };

        var saved = await _scheduleRepository.UpdateTakingScheduleAsync(schedule, updateFlags, cancellationToken);
        return (ScheduleId)(saved?.Id ?? 0);
    }

    public async Task<OneOf<True, NotFound>> RemoveSchedule(long scheduleId,
        CancellationToken cancellationToken = default)
    {
        var removeResult = await
            _scheduleRepository.RemoveTakingScheduleAsync(scheduleId, _currentUserIdentifier.UserId, cancellationToken);
        if (removeResult != RemoveOperationResult.Removed)
        {
            return new NotFound("User doesn't have schedules with provided ID");
        }

        return new True();
    }

    public async Task<OneOf<RepeatId, NotFound, InvalidInput>> CreateRepeatAsync(NewScheduleRepeat newRepeat,
        CancellationToken cancellationToken = default)
    {
        var error = await GetScheduleParametersErrorsAsync(
            newSchedule.UserMedicamentId, newSchedule.GlobalMedicamentId, cancellationToken);
        if (error != null)
        {
            return error.Value.IsT0 ? error.Value.AsT0 : error.Value.AsT1;
        };

        var schedule = new TakingSchedulePlain
        {
            UserProfileId = _currentUserIdentifier.UserId,
            GlobalMedicamentId = newSchedule.GlobalMedicamentId,
            UserMedicamentId = newSchedule.UserMedicamentId,
            Information = newSchedule.Information?.Trim(),
            CreatedAt = DateTime.UtcNow,
            Enabled = newSchedule.Enabled
        };

        var saved = await _scheduleRepository.CreateTakingScheduleAsync(schedule, cancellationToken);
        return (ScheduleId)(saved?.Id ?? 0);
    }

    public async Task<OneOf<RepeatId, NotFound, InvalidInput>> UpdateRepeatAsync(ScheduleRepeatUpdate update,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<OneOf<True, NotFound>> RemoveRepeatAsync(long repeatId,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<OneOf<True, NotFound, InvalidInput>> AddOrUpdateShareAsync(ScheduleShareUpdate newShare,
        CancellationToken cancellationToken = default)
    {
        var notFound = new NotFound();
        var scheduleExists = await _scheduleRepository.DoesScheduleExistsAsync(newShare.ScheduleId,
            _currentUserIdentifier.UserId, cancellationToken);
        if (!scheduleExists)
        {
            notFound.Add("User doesn't have schedules with provided ID");
        }

        var isCommonContact = await _contactRepository.IsContactCommon(_currentUserIdentifier.UserId,
            newShare.CommonContactProfileId, cancellationToken);
        if (!isCommonContact.HasValue || !isCommonContact.Value)
        {
            notFound.Add("User doesn't have a common contact with provided ID");
        }

        if (notFound.HasMessages) return notFound;

        var share = new ScheduleSharePlain
        {
            ScheduleId = newShare.ScheduleId,
            ShareUserProfileId = newShare.CommonContactProfileId,
            Comment = newShare.Comment
        };

        _ = await _shareRepository.AddOrUpdateShareAsync(share, cancellationToken);
        return new True();
    }

    public async Task<OneOf<True, NotFound>> RemoveShareAsync(long scheduleId, long contactProfileId,
        CancellationToken cancellationToken = default)
    {
        _ = await _shareRepository.RemoveScheduleShareAsync(scheduleId, contactProfileId, cancellationToken);
        return new True();
    }


    private async Task<OneOf<NotFound, InvalidInput>?> GetRepeatParametersErrorsAsync(long? scheduleId, long? userDrugId, int? globalDrugId,
        CancellationToken cancellationToken = default)
    {
        if (userDrugId == null && globalDrugId == null)
        {
            return new InvalidInput("Either global medicament or User medicament must be defined");
        }

        var globalDrugPassed = globalDrugId == null ||
                               await _drugRepository.DoesMedicamentExistAsync(globalDrugId.Value, cancellationToken);

        var userDrugPassed = userDrugId == null ||
                             await _userDrugRepository.DoesMedicamentExistAsync(
                                 _currentUserIdentifier.UserId, userDrugId.Value, cancellationToken);

        var notFound = new NotFound();
        if (scheduleId != null)
        {
            var exists = await _scheduleRepository.DoesScheduleExistsAsync(scheduleId.Value, _currentUserIdentifier.UserId, cancellationToken);
            return new InvalidInput("Either global medicament or User medicament must be defined");
        }

        if (!globalDrugPassed || !userDrugPassed)
        {
            return new NotFound("Medicament not found");
        }

        return null;
    }

    private async Task<OneOf<NotFound, InvalidInput>?> GetScheduleParametersErrorsAsync(long? scheduleId, long? userDrugId, int? globalDrugId,
        CancellationToken cancellationToken = default)
    {
        if (userDrugId == null && globalDrugId == null)
        {
            return new InvalidInput("Either global medicament or User medicament must be defined");
        }

        var globalDrugPassed = globalDrugId == null ||
                               await _drugRepository.DoesMedicamentExistAsync(globalDrugId.Value, cancellationToken);

        var userDrugPassed = userDrugId == null ||
                             await _userDrugRepository.DoesMedicamentExistAsync(
                                 _currentUserIdentifier.UserId, userDrugId.Value, cancellationToken);

        var notFound = new NotFound();
        if (scheduleId != null)
        {
            var exists = await _scheduleRepository.DoesScheduleExistsAsync(scheduleId.Value, _currentUserIdentifier.UserId, cancellationToken);
            return new InvalidInput("Either global medicament or User medicament must be defined");
        }

        if (!globalDrugPassed || !userDrugPassed)
        {
            return new NotFound("Medicament not found");
        }

        return null;
    }
}