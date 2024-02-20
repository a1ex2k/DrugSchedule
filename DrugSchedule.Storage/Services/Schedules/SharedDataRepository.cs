using DrugSchedule.Storage.Data;
using DrugSchedule.Storage.Data.Entities;
using DrugSchedule.Storage.Extensions;
using DrugSchedule.StorageContract.Abstractions;
using DrugSchedule.StorageContract.Data;
using DrugSchedule.StorageContract.Data.Schedule.Tool;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace DrugSchedule.Storage.Services;

public class ScheduleSpecialRepository : ISharedDataRepository
{
    private readonly DrugScheduleContext _dbContext;
    private readonly ILogger<ScheduleSpecialRepository> _logger;

    public ScheduleSpecialRepository(DrugScheduleContext dbContext, ILogger<ScheduleSpecialRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }


    public async Task<Contract.TakingScheduleSimple?> GetScheduleSimpleAsync(long id, long userId, CancellationToken cancellationToken = default)
    {
        var schedule = await _dbContext.MedicamentTakingSchedules
            .Where(s => s.Id == id)
            .Where(s => s.UserProfileId == userId
                        || s.ScheduleShares.Any(share => share.ShareWithContact!.ContactProfileId == userId))
            .Select(EntityMapExpressions.ToScheduleSimple(_dbContext, userId))
            .FirstOrDefaultAsync(cancellationToken);
        return schedule;
    }


    public async Task<List<Contract.TakingScheduleSimple>> GetSchedulesSimpleAsync(Contract.TakingScheduleFilter filter, long userId, CancellationToken cancellationToken = default)
    {
        var schedulesQuery = _dbContext.MedicamentTakingSchedules
            .WithFilter(s => s.Id, filter.IdFilter)
            .WhereIf(filter.UserMedicamentIdFilter != null,
                s => filter.UserMedicamentIdFilter!.Contains((long)s.UserMedicamentId!))
            .WhereIf(filter.GlobalMedicamentIdFilter != null,
                s => filter.GlobalMedicamentIdFilter!.Contains((int)s.GlobalMedicamentId!))
            .WithFilter(s => s.Enabled, filter.EnabledFilter);

        if (filter.OwnedOnly)
        {
            schedulesQuery = schedulesQuery.Where(s => s.UserProfileId == userId);
        }
        else if (filter.ContactProfileId == null)
        {
            schedulesQuery = schedulesQuery.Where(s => s.UserProfileId == userId
                                                       || s.ScheduleShares.Any(share => share.ShareWithContact!.ContactProfileId == userId));
        }
        else if (filter.ContactProfileId != null)
        {
            schedulesQuery = schedulesQuery.Where(s => s.ScheduleShares
                .Any(share => share.ShareWithContact!.ContactProfileId == userId));
        }

        var schedules = await schedulesQuery.OrderByDescending(s => s.CreatedAt)
            .WithPaging(filter)
            .Select(EntityMapExpressions.ToScheduleSimple(_dbContext, userId))
            .ToListAsync(cancellationToken);

        return schedules;
    }


    public async Task<List<Contract.TakingScheduleSimple>> SearchForOwnedOrSharedAsync(long userId, ScheduleSearch searchParams, CancellationToken cancellationToken = default)
    {
        var schedules = await _dbContext.MedicamentTakingSchedules
            .Where(s => s.UserProfileId == userId
                        || s.ScheduleShares.Any(share =>
                            share.ShareWithContact!.ContactProfileId == userId))
            .Where(s => s.Information!.Contains(searchParams.SubString)
                        || s.GlobalMedicament!.Name.Contains(searchParams.SubString)
                        || s.GlobalMedicament!.ReleaseForm!.Name.Contains(searchParams.SubString)
                        || s.UserMedicament!.Name.Contains(searchParams.SubString)
                        || s.UserMedicament!.ReleaseForm!.Contains(searchParams.SubString))
            .OrderByDescending(s => s.CreatedAt)
            .WithPaging(searchParams)
            .Select(EntityMapExpressions.ToScheduleSimple(_dbContext, userId))
            .ToListAsync(cancellationToken);

        return schedules;
    }


    public async Task<Contract.TakingScheduleExtended?> GetScheduleExtendedAsync(long id, long userId, CancellationToken cancellationToken = default)
    {
        var schedule = await _dbContext.MedicamentTakingSchedules
            .Where(s => s.Id == id)
            .Where(s => s.UserProfileId == userId
                        || s.ScheduleShares.Any(share =>
                            share.ShareWithContact!.ContactProfileId == userId))
            .Select(EntityMapExpressions.ToScheduleExtended(_dbContext, userId))
            .FirstOrDefaultAsync(cancellationToken);

        return schedule;
    }


    public async Task<List<Contract.TakingScheduleExtended>> GetSchedulesExtendedAsync(Contract.TakingScheduleFilter filter, long userId, CancellationToken cancellationToken = default)
    {
        var schedulesQuery = _dbContext.MedicamentTakingSchedules
            .WithFilter(s => s.Id, filter.IdFilter)
            .WhereIf(filter.UserMedicamentIdFilter != null,
                s => filter.UserMedicamentIdFilter!.Contains((long)s.UserMedicamentId!))
            .WhereIf(filter.GlobalMedicamentIdFilter != null,
                s => filter.GlobalMedicamentIdFilter!.Contains((int)s.GlobalMedicamentId!))
            .WithFilter(s => s.Enabled, filter.EnabledFilter);

        if (filter.OwnedOnly)
        {
            schedulesQuery = schedulesQuery.Where(s => s.UserProfileId == userId);
        }
        else if (filter.ContactProfileId == null)
        {
            schedulesQuery = schedulesQuery.Where(s => s.UserProfileId == userId
                                                       || s.ScheduleShares.Any(share => share.ShareWithContact!.ContactProfileId == userId));
        }
        else if (filter.ContactProfileId != null)
        {
            schedulesQuery = schedulesQuery.Where(s => s.ScheduleShares
                .Any(share => share.ShareWithContact!.ContactProfileId == userId));
        }

        var schedules = await schedulesQuery
            .OrderByDescending(s => s.CreatedAt)
            .WithPaging(filter)
            .Select(EntityMapExpressions.ToScheduleExtended(_dbContext, userId))
            .ToListAsync(cancellationToken);

        return schedules;
    }

    public async Task<Contract.UserMedicamentExtended?> GetSharedUserMedicament(long userMedicamentId, long mustBeSharedWithProfileId,
        CancellationToken cancellationToken = default)
    {
        var medicament = await _dbContext.UserMedicaments
            .Where(m => m.Id == userMedicamentId)
            .Where(m => _dbContext.ScheduleShare
                .Any(s => s.ShareWithContact!.UserProfileId == mustBeSharedWithProfileId))
            .Select(EntityMapExpressions.ToUserMedicamentExtended(true))
            .FirstOrDefaultAsync(cancellationToken);

        return medicament;
    }

    public async Task<List<long>> GetSharedScheduleIds(long userId, CancellationToken cancellationToken = default)
    {
        var ids = await _dbContext.MedicamentTakingSchedules
            .Where(m => _dbContext.ScheduleShare
                .Any(s => s.ShareWithContact!.UserProfileId == userId))
            .Select(s => s.Id)
            .ToListAsync(cancellationToken);

        return ids;
    }


    public async Task<List<Contract.TakingСonfirmationExtended>> GetTakingConfirmationsAsync(Contract.TakingConfirmationFilter filter, 
        CancellationToken cancellationToken = default)
    {
        var confirmations = await _dbContext.TakingСonfirmations
            .WithFilter(c => c.ScheduleRepeatId, filter.RepeatIds)
            .Where(c => c.ScheduleRepeat!.MedicamentTakingScheduleId == filter.ScheduleId)
            .WhereIf(filter.ForDateMin != null, c => c.ForDate >= filter.ForDateMin)
            .WhereIf(filter.ForDateMax != null, c => c.ForDate <= filter.ForDateMax)
            .OrderByDescending(c => c.ForDate)
            .ThenByDescending(c => c.ForTimeOfDay)
            .ThenByDescending(c => c.ForTime)
            .WithPaging(filter)
            .Select(EntityMapExpressions.ToScheduleConfirmation)
            .ToListAsync(cancellationToken);

        return confirmations;
    }


    public async Task<Contract.ScheduleAccessCheck?> GetOwnOrSharedScheduleIdAsync(long scheduleId, long ownerOrShareProfileId,
        CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.MedicamentTakingSchedules
            .Where(s => s.Id == scheduleId)
            .Select(s => new Contract.ScheduleAccessCheck
            {
                ScheduleId = s.Id,
                OwnerId = s.UserProfileId,
                IsSharedWith = s.ScheduleShares.AsQueryable()
                    .Any(ss => ss.ShareWithContact!.UserProfileId == ownerOrShareProfileId)
            })
            .FirstOrDefaultAsync(cancellationToken);

        return result;
    }


    public async Task<List<Contract.ScheduleAccessCheck>> GetOwnOrSharedSchedulesIdsAsync(List<long>? scheduleIds, long ownerOrShareProfileId,
        CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.MedicamentTakingSchedules
            .WithFilter(s => s.Id, scheduleIds)
            .Select(s => new Contract.ScheduleAccessCheck
            {
                ScheduleId = s.Id,
                OwnerId = s.UserProfileId,
                IsSharedWith = s.ScheduleShares.AsQueryable()
                    .Any(ss => ss.ShareWithContact!.UserProfileId == ownerOrShareProfileId)
            })
            .ToListAsync(cancellationToken);

        return result;
    }
}