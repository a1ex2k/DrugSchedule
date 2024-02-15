using DrugSchedule.Storage.Data;
using DrugSchedule.Storage.Data.Entities;
using DrugSchedule.Storage.Extensions;
using DrugSchedule.StorageContract.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DrugSchedule.Storage.Services;

public class ScheduleSpecialRepository : IScheduleSpecialRepository
{
    private readonly DrugScheduleContext _dbContext;
    private readonly ILogger<ScheduleSpecialRepository> _logger;

    public ScheduleSpecialRepository(DrugScheduleContext dbContext, ILogger<ScheduleSpecialRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }


    public async Task<Contract.TakingScheduleSimple?> GetScheduleSimpleAsync(long ownerOrShareProfileId, long id,
        CancellationToken cancellationToken = default)
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


    public async Task<List<Contract.TakingScheduleSimple>> GetSchedulesSimpleAsync(long ownerOrShareProfileId,
        Contract.TakingScheduleFilter filter,
        CancellationToken cancellationToken = default)
    {
        var schedules = await _dbContext.MedicamentTakingSchedules
            .Where(s => s.UserProfileId == ownerOrShareProfileId
                        || s.ScheduleShares.Any(share =>
                            share.ShareWithContact!.ContactProfileId == ownerOrShareProfileId))
            .WithFilter(s => s.Id, filter.IdFilter)
            .WhereIf(filter.UserMedicamentIdFilter != null,
                s => filter.UserMedicamentIdFilter!.Contains((long)s.UserMedicamentId!))
            .WhereIf(filter.GlobalMedicamentIdFilter != null,
                s => filter.GlobalMedicamentIdFilter!.Contains((int)s.GlobalMedicamentId!))
            .WithFilter(s => s.Enabled, filter.EnabledFilter)
            .OrderBy(s => s.Id)
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
            .Select(EntityMapExpressions.ToScheduleSimple)
            .ToListAsync(cancellationToken);

        return schedules;
    }


    public async Task<Contract.TakingScheduleExtended?> GetScheduleExtendedAsync(long ownerOrShareProfileId, long id,
        CancellationToken cancellationToken = default)
    {
        var schedule = await _dbContext.MedicamentTakingSchedules
            .Where(s => s.Id == ownerOrShareProfileId)
            .Where(s => s.UserProfileId == ownerOrShareProfileId
                        || s.ScheduleShares.Any(share =>
                            share.ShareWithContact!.ContactProfileId == ownerOrShareProfileId))
            .Select(EntityMapExpressions.ToScheduleExtended(_dbContext))
            .FirstOrDefaultAsync(cancellationToken);

        return schedule;
    }


    public async Task<List<Contract.TakingScheduleExtended>> GetSchedulesExtendedAsync(long ownerOrShareProfileId,
        Contract.TakingScheduleFilter filter,
        CancellationToken cancellationToken = default)
    {
        var schedules = await _dbContext.MedicamentTakingSchedules
            .Where(s => s.UserProfileId == ownerOrShareProfileId
                        || s.ScheduleShares.Any(share =>
                            share.ShareWithContact!.ContactProfileId == ownerOrShareProfileId))
            .WithFilter(s => s.Id, filter.IdFilter)
            .WhereIf(filter.UserMedicamentIdFilter != null,
                s => filter.UserMedicamentIdFilter!.Contains((long)s.UserMedicamentId!))
            .WhereIf(filter.GlobalMedicamentIdFilter != null,
                s => filter.GlobalMedicamentIdFilter!.Contains((int)s.GlobalMedicamentId!))
            .WithFilter(s => s.Enabled, filter.EnabledFilter)
            .OrderBy(s => s.Id)
            .WithPaging(filter)
            .Select(EntityMapExpressions.ToScheduleExtended(_dbContext))
            .ToListAsync(cancellationToken);

        return schedules;
    }

    public async Task<List<Contract.ScheduleShareExtended>> GetScheduleSharesExtendedAsync(Contract.TakingConfirmationFilter filter,
        CancellationToken cancellationToken = default)
    {
        var confirmations = await _dbContext.TakingСonfirmations
            .Where(c => c.ScheduleRepeatId == filter.RepeatId)
            .Where(c => c.ScheduleRepeat!.MedicamentTakingScheduleId == filter.ScheduleId)
            
            .OrderByDescending()
            .Select(r => r.TakingСonfirmations.AsQueryable()
            .Select(EntityMapExpressions.ToScheduleConfirmation))
            .FirstOrDefaultAsync(cancellationToken);
        return schedule;
    }


    public async Task<long> GetOwnerUserIdAsync(long scheduleId, CancellationToken cancellationToken = default)
    {
        var userId = _dbContext.MedicamentTakingSchedules
            .Where(s => s.Id == scheduleId)
            .Select(s => s.UserProfileId)
            .FirstOrDefault();
        return userId;
    }


    public async Task<List<(long Id, long OwnerId)>> GetOwnOrSharedSchedulesIdsAsync(long ownerOrShareProfileId,
        CancellationToken cancellationToken = default)
    {
        var idList = await _dbContext.MedicamentTakingSchedules
            .Where(s => s.UserProfileId == ownerOrShareProfileId
                        || s.ScheduleShares.Any(share =>
                            share.ShareWithContact!.ContactProfileId == ownerOrShareProfileId))
            .Select(s => new { s.Id, s.UserProfileId })
            .ToListAsync(cancellationToken);

        var result = idList.ConvertAll(x => (x.Id, x.UserProfileId));
        return result;
    }
}