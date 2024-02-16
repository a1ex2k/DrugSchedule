using DrugSchedule.Services.Converters;
using DrugSchedule.Services.Models;
using DrugSchedule.Services.Services.Abstractions;
using DrugSchedule.StorageContract.Abstractions;
using DrugSchedule.StorageContract.Data;
using TakingScheduleExtended = DrugSchedule.Services.Models.TakingScheduleExtended;
using UserContactSimple = DrugSchedule.StorageContract.Data.UserContactSimple;

namespace DrugSchedule.Services.Services;

public class ScheduleReadService : IScheduleReadService
{
    private readonly ISharedDataRepository _sharedDataRepository;
    private readonly ICurrentUserIdentifier _currentUserIdentifier;
    private readonly IScheduleConverter _converter;

    public ScheduleReadService(ISharedDataRepository scheduleRepository, ICurrentUserIdentifier currentUserIdentifier, IScheduleConverter converter)
    {
        _sharedDataRepository = scheduleRepository;
        _currentUserIdentifier = currentUserIdentifier;
        _converter = converter;
    }


    public async Task<OneOf<ScheduleSimpleCollection, InvalidInput>> SearchForScheduleAsync(string searchString, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(searchString) || searchString.Trim().Length < 3)
        {
            return new InvalidInput("Search value must be at least 3 not whitespace characters long");
        }

        var foundSchedules =
            await _sharedDataRepository.SearchForOwnedOrSharedAsync(_currentUserIdentifier.UserProfileId,
                searchString, cancellationToken);

        return _converter.ToScheduleSimpleCollection(foundSchedules);
    }


    public async Task<OneOf<ScheduleSimple, NotFound>> GetScheduleSimpleAsync(long id, CancellationToken cancellationToken = default)
    {
        var schedule = await _sharedDataRepository.GetScheduleSimpleAsync(id,
            _currentUserIdentifier.UserProfileId, cancellationToken);

        if (schedule == null)
        {
            return new NotFound("Schedule was not found or current user doesn't have permissions to access");
        }

        return _converter.ToScheduleSimple(schedule!);
    }


    public async Task<ScheduleSimpleCollection> GetSchedulesSimpleAsync(TakingScheduleFilter filter, CancellationToken cancellationToken = default)
    {
        var schedules = await _sharedDataRepository.GetSchedulesSimpleAsync(
            filter, _currentUserIdentifier.UserProfileId, cancellationToken);

        return _converter.ToScheduleSimpleCollection(schedules);
    }


    public async Task<OneOf<TakingScheduleExtended, NotFound>> GetScheduleExtendedAsync(long id, CancellationToken cancellationToken = default)
    {
        var schedule = await _sharedDataRepository.GetScheduleExtendedAsync(id,
            _currentUserIdentifier.UserProfileId, cancellationToken);

        if (schedule == null)
        {
            return new NotFound("Schedule was not found or current user doesn't have permissions to access");
        }

        return _converter.ToScheduleExtended(schedule);
    }


    public async Task<ScheduleExtendedCollection> GetSchedulesExtendedAsync(TakingScheduleFilter filter, CancellationToken cancellationToken = default)
    {
        var schedules = await _sharedDataRepository.GetSchedulesExtendedAsync(
            filter, _currentUserIdentifier.UserProfileId, cancellationToken);

        return _converter.ToScheduleExtendedCollection(schedules);
    }


    public async Task<OneOf<TakingСonfirmationCollection, NotFound>> GetTakingConfirmationsAsync(TakingConfirmationFilter filter, CancellationToken cancellationToken = default)
    {
        var checkResult =
            await _sharedDataRepository.GetOwnOrSharedSchedulesIdsAsync(filter.ScheduleId, _currentUserIdentifier.UserProfileId,
                cancellationToken);

        if (checkResult == null)
        {
            return new NotFound("Schedule was not found or current user doesn't have permissions to access");
        }

        var confirmations = await _sharedDataRepository.GetTakingConfirmationsAsync(filter, cancellationToken);
        return _converter.ToСonfirmationCollection(confirmations);
    }
    }