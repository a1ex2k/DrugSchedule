﻿using DrugSchedule.Services.Converters;
using DrugSchedule.Services.Models;
using DrugSchedule.Services.Services.Abstractions;
using DrugSchedule.Services.Utils;
using DrugSchedule.StorageContract.Abstractions;
using DrugSchedule.StorageContract.Data;
using System;
using TakingScheduleExtended = DrugSchedule.Services.Models.TakingScheduleExtended;

namespace DrugSchedule.Services.Services;

public class ScheduleService : IScheduleService
{
    private readonly ISharedDataRepository _sharedDataRepository;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly ICurrentUserIdentifier _currentUserIdentifier;
    private readonly ITimetableBuilder _timetableBuilder;
    private readonly IScheduleConverter _converter;


    public ScheduleService(ISharedDataRepository scheduleRepository, ICurrentUserIdentifier currentUserIdentifier, IScheduleConverter converter, ITimetableBuilder timetableBuilder, IScheduleRepository scheduleRepository1)
    {
        _sharedDataRepository = scheduleRepository;
        _currentUserIdentifier = currentUserIdentifier;
        _converter = converter;
        _timetableBuilder = timetableBuilder;
        _scheduleRepository = scheduleRepository1;
    }


    public async Task<OneOf<ScheduleSimpleCollection, InvalidInput>> SearchForScheduleAsync(string searchString, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(searchString) || searchString.Trim().Length < 3)
        {
            return new InvalidInput("Search value must be at least 3 not whitespace characters long");
        }

        var foundSchedules =
            await _sharedDataRepository.SearchForOwnedOrSharedAsync(_currentUserIdentifier.UserId,
                searchString, cancellationToken);

        return _converter.ToScheduleSimpleCollection(foundSchedules);
    }


    public async Task<OneOf<ScheduleSimple, NotFound>> GetScheduleSimpleAsync(long id, CancellationToken cancellationToken = default)
    {
        var schedule = await _sharedDataRepository.GetScheduleSimpleAsync(id,
            _currentUserIdentifier.UserId, cancellationToken);

        if (schedule == null)
        {
            return new NotFound("Schedule was not found or current user doesn't have permissions to access");
        }

        return _converter.ToScheduleSimple(schedule!);
    }


    public async Task<ScheduleSimpleCollection> GetSchedulesSimpleAsync(TakingScheduleFilter filter, CancellationToken cancellationToken = default)
    {
        var schedules = await _sharedDataRepository.GetSchedulesSimpleAsync(
            filter, _currentUserIdentifier.UserId, cancellationToken);

        return _converter.ToScheduleSimpleCollection(schedules);
    }


    public async Task<OneOf<TakingScheduleExtended, NotFound>> GetScheduleExtendedAsync(long id, CancellationToken cancellationToken = default)
    {
        var schedule = await _sharedDataRepository.GetScheduleExtendedAsync(id,
            _currentUserIdentifier.UserId, cancellationToken);

        if (schedule == null)
        {
            return new NotFound("Schedule was not found or current user doesn't have permissions to access");
        }

        return _converter.ToScheduleExtended(schedule);
    }


    public async Task<ScheduleExtendedCollection> GetSchedulesExtendedAsync(TakingScheduleFilter filter, CancellationToken cancellationToken = default)
    {
        var schedules = await _sharedDataRepository.GetSchedulesExtendedAsync(
            filter, _currentUserIdentifier.UserId, cancellationToken);

        return _converter.ToScheduleExtendedCollection(schedules);
    }


    public async Task<OneOf<TakingСonfirmationCollection, NotFound>> GetTakingConfirmationsAsync(TakingConfirmationFilter filter, CancellationToken cancellationToken = default)
    {
        var isAccessable = await DoesExistAndUserHasAccessAsync(filter.ScheduleId, cancellationToken);
        if (!isAccessable)
        {
            return new NotFound("Schedule was not found or current user doesn't have permissions to access");
        }

        var confirmations = await _sharedDataRepository.GetTakingConfirmationsAsync(filter, cancellationToken);
        return _converter.ToСonfirmationCollection(confirmations);
    }


    public async Task<OneOf<Timetable, InvalidInput>> GetTimetableAsync(DateOnly minDate, DateOnly maxDate, CancellationToken cancellationToken = default)
    {
        var datesAreValid = minDate <= maxDate && minDate.AddDays(32) >= maxDate;
        if (!datesAreValid)
        {
            return new InvalidInput("Invalid dates. Minimum and maximum dates difference must be 32 days or less");
        }

        var scheduleIds =
            await _scheduleRepository.GetUserSchedulesIdsAsync(_currentUserIdentifier.UserId, cancellationToken);

        if (scheduleIds.Count == 0)
        {
            return new Timetable();
        }

        var timetableEntries =
            await _timetableBuilder.GetScheduleTimetableAsync(scheduleIds, minDate, maxDate, true, cancellationToken);

        return new Timetable { TimetableEntries = timetableEntries };
    }


    public async Task<OneOf<Timetable, NotFound, InvalidInput>> GetScheduleTimetableAsync(long scheduleId, DateOnly minDate, DateOnly maxDate, CancellationToken cancellationToken = default)
    {
        var datesAreValid = minDate <= maxDate && minDate.AddDays(32) >= maxDate;
        if (!datesAreValid)
        {
            return new InvalidInput("Invalid dates. Minimum and maximum dates difference must be 32 days or less");
        }

        var hasAccess = await DoesExistAndUserHasAccessAsync(scheduleId, cancellationToken);
        if (!hasAccess)
        {
            return new NotFound("Schedule was not found or current user doesn't have permissions to access");
        }

        var timetableEntries =
            await _timetableBuilder.GetScheduleTimetableAsync([scheduleId], minDate, maxDate, true, cancellationToken);

        return new Timetable { TimetableEntries = timetableEntries };
    }


    private async Task<bool> DoesExistAndUserHasAccessAsync(long scheduleId, CancellationToken cancellationToken)
    {
        var checkResult =
            await _sharedDataRepository.GetOwnOrSharedScheduleIdAsync(scheduleId, _currentUserIdentifier.UserId,
                cancellationToken);

        return checkResult is not null && 
               (checkResult.OwnerId == _currentUserIdentifier.UserId || checkResult.IsSharedWith);
    }
}