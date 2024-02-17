using DrugSchedule.Storage.Data;
using DrugSchedule.Storage.Extensions;
using DrugSchedule.StorageContract.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DrugSchedule.Storage.Services;

public class ScheduleRepository : IScheduleRepository
{
    private readonly DrugScheduleContext _dbContext;
    private readonly ILogger<ScheduleRepository> _logger;

    public ScheduleRepository(DrugScheduleContext dbContext, ILogger<ScheduleRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Contract.TakingSchedulePlain?> GetTakingScheduleAsync(long id,
        CancellationToken cancellationToken = default)
    {
        var schedule = await _dbContext.MedicamentTakingSchedules
            .Where(s => s.Id == id)
            .Select(EntityMapExpressions.ToSchedulePlain)
            .FirstOrDefaultAsync(cancellationToken);
        return schedule;
    }

    public async Task<Contract.TakingSchedulePlain?> GetTakingScheduleAsync(long id, long userId,
        CancellationToken cancellationToken = default)
    {
        var schedule = await _dbContext.MedicamentTakingSchedules
            .Where(s => s.Id == id && s.UserProfileId == userId)
            .Select(EntityMapExpressions.ToSchedulePlain)
            .FirstOrDefaultAsync(cancellationToken);
        return schedule;
    }

    public async Task<bool> DoesScheduleExistsAsync(long id, long userId, CancellationToken cancellationToken = default)
    {
        var scheduleExists = await _dbContext.MedicamentTakingSchedules
            .Where(s => s.Id == id && s.UserProfileId == userId)
            .AnyAsync(cancellationToken);
        return scheduleExists;
    }

    public async Task<List<long>> GetUserSchedulesIdsAsync(long userId, CancellationToken cancellationToken = default)
    {
        var ids = await _dbContext.MedicamentTakingSchedules
            .Where(s => s.UserProfileId == userId)
            .Select(s => s.Id)
            .ToListAsync(cancellationToken);

        return ids;
    }

    public async Task<List<Contract.TakingSchedulePlain>> GetTakingSchedulesAsync(Contract.TakingScheduleFilter filter,
        long userId, CancellationToken cancellationToken = default)
    {
        var schedules = await _dbContext.MedicamentTakingSchedules
            .Where(s => s.UserProfileId == userId)
            .WithFilter(s => s.Id, filter.IdFilter)
            .WhereIf(filter.UserMedicamentIdFilter != null,
                s => filter.UserMedicamentIdFilter!.Contains((long)s.UserMedicamentId!))
            .WhereIf(filter.GlobalMedicamentIdFilter != null,
                s => filter.GlobalMedicamentIdFilter!.Contains((int)s.GlobalMedicamentId!))
            .WithFilter(s => s.Enabled, filter.EnabledFilter)
            .OrderBy(s => s.Id)
            .WithPaging(filter)
            .Select(EntityMapExpressions.ToSchedulePlain)
            .ToListAsync(cancellationToken);

        return schedules;
    }

    public async Task<Contract.TakingSchedulePlain?> CreateTakingScheduleAsync(
        Contract.TakingSchedulePlain takingSchedule, CancellationToken cancellationToken = default)
    {
        var entity = new Entities.MedicamentTakingSchedule
        {
            Id = 0,
            UserProfileId = takingSchedule.UserProfileId,
            GlobalMedicamentId = takingSchedule.GlobalMedicamentId,
            UserMedicamentId = takingSchedule.UserMedicamentId,
            Information = takingSchedule.Information,
            CreatedAt = takingSchedule.CreatedAt,
            Enabled = takingSchedule.Enabled,
        };

        var saved = await _dbContext.TrySaveChangesAsync(_logger, cancellationToken);
        return saved ? entity.ToContractModel() : null;
    }

    public async Task<Contract.TakingSchedulePlain?> UpdateTakingScheduleAsync(
        Contract.TakingSchedulePlain takingSchedule, Contract.TakingScheduleUpdateFlags updateFlags,
        CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.MedicamentTakingSchedules
            .FirstOrDefaultAsync(s => s.Id == takingSchedule.Id && s.UserProfileId == takingSchedule.UserProfileId,
                cancellationToken);

        if (entity == null) return null;

        var entry = _dbContext.Entry(entity);
        entry.UpdateIf(e => e.UserMedicamentId, takingSchedule.UserMedicamentId, updateFlags.UserMedicamentId);
        entry.UpdateIf(e => e.GlobalMedicamentId, takingSchedule.GlobalMedicamentId, updateFlags.GlobalMedicamentId);
        entry.UpdateIf(e => e.Information, takingSchedule.Information, updateFlags.Information);
        entry.UpdateIf(e => e.Enabled, takingSchedule.Enabled, updateFlags.Enabled);

        var saved = await _dbContext.TrySaveChangesAsync(_logger, cancellationToken);
        return saved ? entity.ToContractModel() : null;
    }

    public async Task<Contract.RemoveOperationResult> RemoveTakingScheduleAsync(long id,
        CancellationToken cancellationToken = default)
    {
        var deletedCount = await _dbContext.MedicamentTakingSchedules
            .Where(s => s.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return deletedCount > 0 ? Contract.RemoveOperationResult.Removed : Contract.RemoveOperationResult.NotFound;
    }
}