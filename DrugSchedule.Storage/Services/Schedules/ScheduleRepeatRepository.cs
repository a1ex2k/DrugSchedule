using DrugSchedule.Storage.Data;
using DrugSchedule.Storage.Extensions;
using DrugSchedule.StorageContract.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DrugSchedule.Storage.Services;

public class ScheduleRepeatRepository : IScheduleRepeatRepository
{
    private readonly DrugScheduleContext _dbContext;
    private readonly ILogger<ScheduleRepeatRepository> _logger;

    public ScheduleRepeatRepository(DrugScheduleContext dbContext, ILogger<ScheduleRepeatRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Contract.ScheduleRepeatPlain?> GetRepeatAsync(long id, CancellationToken cancellationToken = default)
    {
        var repeat = await _dbContext.ScheduleRepeat
            .AsNoTracking()
            .Select(EntityMapExpressions.ToScheduleRepeatPlain)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        return repeat;
    }

    public async Task<Contract.ScheduleRepeatPlain?> GetRepeatAsync(long id, long scheduleId, long userProfileId, CancellationToken cancellationToken = default)
    {
        var repeat = await _dbContext.ScheduleRepeat
            .AsNoTracking()
            .Where(r => r.Id == id)
            .Where(r => r.MedicamentTakingScheduleId == scheduleId)
            .Where(r => r.MedicamentTakingSchedule!.UserProfileId == userProfileId)
            .Select(EntityMapExpressions.ToScheduleRepeatPlain)
            .FirstOrDefaultAsync(cancellationToken);
        return repeat;
    }

    public async Task<bool> DoesRepeatExistAsync(long id, long scheduleId, long userProfileId, CancellationToken cancellationToken = default)
    {
        var repeatExists = await _dbContext.ScheduleRepeat
            .Where(r => r.Id == id)
            .Where(r => r.MedicamentTakingScheduleId == scheduleId)
            .Where(r => r.MedicamentTakingSchedule!.UserProfileId == userProfileId)
            .AnyAsync(cancellationToken);

        return repeatExists;
    }

    public async Task<List<Contract.ScheduleRepeatPlain>> GetRepeatsAsync(long scheduleId, CancellationToken cancellationToken = default)
    {
        var repeats = await _dbContext.ScheduleRepeat
            .AsNoTracking()
            .Where(r => r.MedicamentTakingScheduleId == scheduleId)
            .Select(EntityMapExpressions.ToScheduleRepeatPlain)
            .ToListAsync(cancellationToken);

        return repeats;
    }

    public async Task<Contract.ScheduleRepeatPlain?> CreateRepeatAsync(Contract.ScheduleRepeatPlain repeat, CancellationToken cancellationToken = default)
    {
        var entity = new Entities.ScheduleRepeat
        {
            Id = 0,
            BeginDate = repeat.BeginDate,
            Time = repeat.Time,
            TimeOfDay = Contract.TimeOfDay.None,
            RepeatDayOfWeek = repeat.RepeatDayOfWeek,
            EndDate = repeat.EndDate,
            MedicamentTakingScheduleId = repeat.MedicamentTakingScheduleId,
            TakingRule = repeat.TakingRule,
        };

        await _dbContext.ScheduleRepeat.AddAsync(entity, cancellationToken);
        var saved = await _dbContext.TrySaveChangesAsync(_logger, cancellationToken);
        return saved ? entity.ToContractModel() : null;
    }

    public async Task<Contract.ScheduleRepeatPlain?> UpdateRepeatAsync(Contract.ScheduleRepeatPlain repeat, Contract.ScheduleRepeatUpdateFlags updateFlags,
        CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.ScheduleRepeat
            .Where(r => r.Id == repeat.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null) return null;

        var entry = _dbContext.Entry(entity);
        entry.UpdateIf(e => e.BeginDate, repeat.BeginDate, updateFlags.BeginDate);
        entry.UpdateIf(e => e.EndDate, repeat.EndDate, updateFlags.EndDate);
        entry.UpdateIf(e => e.TimeOfDay, repeat.TimeOfDay, updateFlags.TimeOfDay);
        entry.UpdateIf(e => e.Time, repeat.Time, updateFlags.Time);
        entry.UpdateIf(e => e.RepeatDayOfWeek, repeat.RepeatDayOfWeek, updateFlags.RepeatDayOfWeek);
        entry.UpdateIf(e => e.TakingRule, repeat.TakingRule, updateFlags.TakingRule);

        var saved = await _dbContext.TrySaveChangesAsync(_logger, cancellationToken);
        return saved ? entity.ToContractModel() : null;
    }

    public async Task<Contract.RemoveOperationResult> RemoveRepeatAsync(long id, CancellationToken cancellationToken = default)
    {
        var deletedCount = await _dbContext.ScheduleRepeat
            .Where(s => s.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return deletedCount > 0 ? Contract.RemoveOperationResult.Removed : Contract.RemoveOperationResult.NotFound;
    }
}