using DrugSchedule.Storage.Data;
using DrugSchedule.Storage.Data.Entities;
using DrugSchedule.Storage.Extensions;
using DrugSchedule.StorageContract.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DrugSchedule.Storage.Services;

public class ScheduleSpecialRepository : IScheduleSpecialRepository//, IScheduleAccessChecker
{
    private readonly DrugScheduleContext _dbContext;
    private readonly ILogger<ScheduleSpecialRepository> _logger;

    public ScheduleSpecialRepository(DrugScheduleContext dbContext, ILogger<ScheduleSpecialRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }


    public async Task<Contract.TakingScheduleSimple?> GetScheduleSimpleAsync(long id, long ownerOrShareProfileId, CancellationToken cancellationToken = default)
    {
        var schedule = await _dbContext.MedicamentTakingSchedules
            .Where(s => s.Id == id)
            .Where(s => s.UserProfileId == ownerOrShareProfileId
                        || s.ScheduleShares.Any(share =>
                            share.ShareWithContact!.ContactProfileId == ownerOrShareProfileId))
            .Select(EntityMapExpressions.ToScheduleSimple)
            .FirstOrDefaultAsync(cancellationToken);
        return schedule;
    }


    public async Task<List<Contract.TakingScheduleSimple>> GetSchedulesSimpleAsync(Contract.TakingScheduleFilter filter, long ownerOrShareProfileId, CancellationToken cancellationToken = default)
    {
        var schedules = await _dbContext.MedicamentTakingSchedules
            .WithFilter(s => s.Id, filter.IdFilter)
            .Where(s => s.UserProfileId == ownerOrShareProfileId
                        || s.ScheduleShares.Any(share =>
                            share.ShareWithContact!.ContactProfileId == ownerOrShareProfileId))
            .WhereIf(filter.UserMedicamentIdFilter != null,
                s => filter.UserMedicamentIdFilter!.Contains((long)s.UserMedicamentId!))
            .WhereIf(filter.GlobalMedicamentIdFilter != null,
                s => filter.GlobalMedicamentIdFilter!.Contains((int)s.GlobalMedicamentId!))
            .WithFilter(s => s.Enabled, filter.EnabledFilter)
            .OrderByDescending(s => s.CreatedAt)
            .WithPaging(filter)
            .Select(EntityMapExpressions.ToScheduleSimple)
            .ToListAsync(cancellationToken);

        return schedules;
    }


    public async Task<List<Contract.TakingScheduleSimple>> SearchForOwnedOrSharedAsync(long ownerOrShareProfileId,
        string searchString,
        CancellationToken cancellationToken = default)
    {
        var schedules = await _dbContext.MedicamentTakingSchedules
            .Where(s => s.UserProfileId == ownerOrShareProfileId
                        || s.ScheduleShares.Any(share =>
                            share.ShareWithContact!.ContactProfileId == ownerOrShareProfileId))
            .Where(s => s.Information!.Contains(searchString)
                        || s.GlobalMedicament!.Name.Contains(searchString)
                        || s.GlobalMedicament!.ReleaseForm!.Name.Contains(searchString)
                        || s.UserMedicament!.Name.Contains(searchString)
                        || s.UserMedicament!.ReleaseForm!.Contains(searchString))
            .OrderByDescending(s => s.CreatedAt)
            .Select(EntityMapExpressions.ToScheduleSimple)
            .ToListAsync(cancellationToken);

        return schedules;
    }


    public async Task<Contract.TakingScheduleExtended?> GetScheduleExtendedAsync(long id, long ownerOrShareProfileId, CancellationToken cancellationToken = default)
    {
        var schedule = await _dbContext.MedicamentTakingSchedules
            .Where(s => s.Id == id)
            .Where(s => s.UserProfileId == ownerOrShareProfileId
                        || s.ScheduleShares.Any(share =>
                            share.ShareWithContact!.ContactProfileId == ownerOrShareProfileId))
            .Select(EntityMapExpressions.ToScheduleExtended(_dbContext))
            .FirstOrDefaultAsync(cancellationToken);

        return schedule;
    }


    public async Task<List<Contract.TakingScheduleExtended>> GetSchedulesExtendedAsync(Contract.TakingScheduleFilter filter, long ownerOrShareProfileId, CancellationToken cancellationToken = default)
    {
        var schedules = await _dbContext.MedicamentTakingSchedules
            .WithFilter(s => s.Id, filter.IdFilter)
            .Where(s => s.UserProfileId == ownerOrShareProfileId
                        || s.ScheduleShares.Any(share =>
                            share.ShareWithContact!.ContactProfileId == ownerOrShareProfileId))
            .WhereIf(filter.UserMedicamentIdFilter != null,
                s => filter.UserMedicamentIdFilter!.Contains((long)s.UserMedicamentId!))
            .WhereIf(filter.GlobalMedicamentIdFilter != null,
                s => filter.GlobalMedicamentIdFilter!.Contains((int)s.GlobalMedicamentId!))
            .WithFilter(s => s.Enabled, filter.EnabledFilter)
            .OrderByDescending(s => s.CreatedAt)
            .WithPaging(filter)
            .Select(EntityMapExpressions.ToScheduleExtended(_dbContext))
            .ToListAsync(cancellationToken);

        return schedules;
    }
    

    public async Task<List<Contract.TakingСonfirmation>> GetTakingConfirmationsAsync(Contract.TakingConfirmationFilter filter, 
        CancellationToken cancellationToken = default)
    {
        var confirmations = await _dbContext.TakingСonfirmations
            .WhereIf(filter.RepeatId.HasValue, c => c.ScheduleRepeatId == filter.RepeatId)
            .Where(c => c.ScheduleRepeat!.MedicamentTakingScheduleId == filter.ScheduleId)
            .OrderByDescending(c => c.CreatedAt)
            .WithPaging(filter)
            .Select(EntityMapExpressions.ToScheduleConfirmation)
            .ToListAsync(cancellationToken);

        return confirmations;
    }


    public async Task<Contract.ScheduleAccessCheck?> GetOwnOrSharedSchedulesIdsAsync(long scheduleId, long ownerOrShareProfileId,
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


    public async Task<List<Contract.ScheduleAccessCheck>> GetOwnOrSharedSchedulesIdsAsync(List<long> scheduleIds, long ownerOrShareProfileId,
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