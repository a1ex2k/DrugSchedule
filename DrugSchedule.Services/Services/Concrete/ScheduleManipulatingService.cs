using System.Runtime.InteropServices;
using DrugSchedule.Services.Models;
using DrugSchedule.Services.Services.Abstractions;
using DrugSchedule.Services.Utils;
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
            Information = newSchedule.Information?.Trim().Limit(),
            CreatedAt = DateTime.UtcNow,
            Enabled = newSchedule.Enabled
        };

        var saved = await _scheduleRepository.CreateTakingScheduleAsync(schedule, cancellationToken);
        if (newSchedule.Shares != null)
        {
            foreach (var share in newSchedule.Shares)
            {
                var isCommon = await _contactRepository.IsContactCommon(_currentUserIdentifier.UserId, share.CommonContactProfileId);
                if (isCommon == true)
                {
                    await _shareRepository.AddOrUpdateShareAsync(new ScheduleSharePlain
                    {
                        ScheduleId = saved.Id,
                        ShareUserProfileId = share.CommonContactProfileId,
                        Comment = share.Comment
                    }, cancellationToken);
                }
            }
        }

        return (ScheduleId)saved!.Id;
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
            Information = update.Information?.Trim().Limit(),
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
        return (ScheduleId)saved!.Id;
    }

    public async Task<OneOf<True, NotFound>> RemoveSchedule(long scheduleId,
        CancellationToken cancellationToken = default)
    {
        var removeResult = await
            _scheduleRepository.RemoveTakingScheduleAsync(scheduleId, _currentUserIdentifier.UserId, cancellationToken);
        if (removeResult != RemoveOperationResult.Removed)
        {
            return new NotFound(ErrorMessages.UserDoesntHaveSchedule);
        }

        return new True();
    }

    public async Task<OneOf<RepeatId, NotFound, InvalidInput>> CreateRepeatAsync(NewScheduleRepeat newRepeat,
        CancellationToken cancellationToken = default)
    {
        var scheduleExists = await _scheduleRepository.DoesScheduleExistsAsync(newRepeat.ScheduleId, _currentUserIdentifier.UserId, cancellationToken);
        if (!scheduleExists) 
        {
            return new NotFound(ErrorMessages.ScheduleNotFound);
        }

        var invalidInput = new InvalidInput();
        if (newRepeat.EndDate != null && newRepeat.EndDate < newRepeat.BeginDate)
        {
            invalidInput.Add(ErrorMessages.ScheduleDatesInvalid);
        }

        if (newRepeat.RepeatDayOfWeek == 0)
        {
            invalidInput.Add(ErrorMessages.RepeatDaysInvalid);
        }

        if (newRepeat.TimeOfDay == TimeOfDay.None && newRepeat.Time == null)
        {
            invalidInput.Add(ErrorMessages.ScheduleDatesInvalid);
        }

        if (invalidInput.HasMessages) return invalidInput;

        var repeat = new ScheduleRepeatPlain
        {
            BeginDate = newRepeat.BeginDate,
            Time = newRepeat.Time,
            TimeOfDay = newRepeat.TimeOfDay,
            RepeatDayOfWeek = newRepeat.RepeatDayOfWeek,
            EndDate = newRepeat.EndDate,
            MedicamentTakingScheduleId = newRepeat.ScheduleId,
            TakingRule = newRepeat.TakingRule?.Trim().Limit()
        };

        var saved = await _repeatRepository.CreateRepeatAsync(repeat, cancellationToken);
        return (RepeatId)saved!.Id;
    }

    public async Task<OneOf<RepeatId, NotFound, InvalidInput>> UpdateRepeatAsync(ScheduleRepeatUpdate repeatUpdate,
        CancellationToken cancellationToken = default)
    {
        var repeatExists = await _repeatRepository.DoesRepeatExistAsync(repeatUpdate.Id, _currentUserIdentifier.UserId, cancellationToken);
        if (!repeatExists)
        {
            return new NotFound(ErrorMessages.UserDoesntHaveRepeat);
        }
        
        var invalidInput = new InvalidInput();
        if (repeatUpdate.EndDate != null && repeatUpdate.EndDate <= repeatUpdate.BeginDate)
        {
            invalidInput.Add(ErrorMessages.ScheduleDatesInvalid);
        }

        if (repeatUpdate.RepeatDayOfWeek == 0)
        {
            invalidInput.Add(ErrorMessages.RepeatDaysInvalid);
        }

        var hasConfirmations = await _confirmationRepository.AnyConfirmationExistsAsync(new () {repeatUpdate.Id}, cancellationToken);
        if (hasConfirmations)
        {
            invalidInput.Add(ErrorMessages.RepeatCannotBeUpdated);
        }

        if (invalidInput.HasMessages) return invalidInput;

        var repeat = new ScheduleRepeatPlain
        {
            Id = repeatUpdate.Id,
            BeginDate = repeatUpdate.BeginDate,
            Time = repeatUpdate.Time,
            TimeOfDay = repeatUpdate.TimeOfDay,
            RepeatDayOfWeek = repeatUpdate.RepeatDayOfWeek,
            EndDate = repeatUpdate.EndDate,
            TakingRule = repeatUpdate.TakingRule?.Trim().Limit()
        };

        var updateFlags = new ScheduleRepeatUpdateFlags
        {
            BeginDate = true,
            Time = true,
            TimeOfDay = true,
            RepeatDayOfWeek = true,
            EndDate = true,
            TakingRule = true
        };

        var saved = await _repeatRepository.UpdateRepeatAsync(repeat, updateFlags, cancellationToken);
        return (RepeatId)saved!.Id;
    }

    public async Task<OneOf<True, NotFound>> RemoveRepeatAsync(long repeatId, CancellationToken cancellationToken = default)
    {
        var repeatExists = await _repeatRepository.DoesRepeatExistAsync(repeatId, _currentUserIdentifier.UserId, cancellationToken);
        if (!repeatExists)
        {
            return new NotFound(ErrorMessages.UserDoesntHaveRepeat);
        }

        _ = await _repeatRepository.RemoveRepeatAsync(repeatId, cancellationToken);
        return new True();
    }
    
    public async Task<OneOf<True, NotFound, InvalidInput>> AddOrUpdateShareAsync(ScheduleShareUpdate newShare,
        CancellationToken cancellationToken = default)
    {
        var notFound = new NotFound();
        var scheduleExists = await _scheduleRepository.DoesScheduleExistsAsync(newShare.ScheduleId,
            _currentUserIdentifier.UserId, cancellationToken);
        if (!scheduleExists)
        {
            notFound.Add(ErrorMessages.UserDoesntHaveSchedule);
        }

        var isCommonContact = await _contactRepository.IsContactCommon(_currentUserIdentifier.UserId,
            newShare.CommonContactProfileId, cancellationToken);
        if (!isCommonContact.HasValue || !isCommonContact.Value)
        {
            notFound.Add(ErrorMessages.NoCommonContact);
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


    private async Task<OneOf<NotFound, InvalidInput>?> GetScheduleParametersErrorsAsync(long? scheduleId, long? userDrugId, int? globalDrugId,
        CancellationToken cancellationToken = default)
    {
        if (userDrugId == null && globalDrugId == null)
        {
            return new InvalidInput(ErrorMessages.ScheduleMedicamentsInvalid);
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
            if (!exists)
            {
                notFound.Add(ErrorMessages.ScheduleNotFound);
            }
        }

        if (!globalDrugPassed || !userDrugPassed)
        {
            notFound.Add(ErrorMessages.MedicamentNotFound);
        }
        
        return notFound.HasMessages ? notFound : (OneOf<NotFound, InvalidInput>?)null;
    }
}