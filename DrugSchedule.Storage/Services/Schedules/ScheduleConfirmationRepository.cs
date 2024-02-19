using DrugSchedule.Storage.Data;
using DrugSchedule.Storage.Extensions;
using DrugSchedule.StorageContract.Abstractions;
using DrugSchedule.StorageContract.Data;
using DrugSchedule.StorageContract.Data.Schedule.Tool;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepeatParameters = System.Collections.Generic.List<(long RepeatId, System.DateOnly[] CaculatedDates)>;

namespace DrugSchedule.Storage.Services;

public class ScheduleConfirmationRepository : IScheduleConfirmationRepository
{
    private readonly DrugScheduleContext _dbContext;
    private readonly ILogger<ScheduleConfirmationRepository> _logger;

    public ScheduleConfirmationRepository(DrugScheduleContext dbContext, ILogger<ScheduleConfirmationRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Contract.TakingСonfirmationPlain?> GetConfirmationAsync(long id, CancellationToken cancellationToken = default)
    {
        var confirmation = await _dbContext.TakingСonfirmations
            .Where(s => s.Id == id)
            .Select(EntityMapExpressions.ToScheduleConfirmationPlain)
            .FirstOrDefaultAsync(cancellationToken);
        return confirmation;
    }

    public async Task<bool> DoesConfirmationExistAsync(long confirmationId, long repeatId, long userId, CancellationToken cancellationToken = default)
    {
        var exists = await _dbContext.TakingСonfirmations
            .Where(s => s.Id == confirmationId)
            .Where(s => s.ScheduleRepeatId == repeatId)
            .Where(s => s.ScheduleRepeat!.MedicamentTakingSchedule!.UserProfileId == userId)
            .AnyAsync(cancellationToken);
        return exists;
    }

    public async Task<Contract.TakingСonfirmationPlain?> CreateConfirmationAsync(Contract.TakingСonfirmationPlain confirmation, CancellationToken cancellationToken = default)
    {
        var entity = new Entities.TakingСonfirmation
        {
            CreatedAt = confirmation.CreatedAt,
            ForDate = confirmation.ForDate,
            ForTime = confirmation.ForTime,
            ForTimeOfDay = confirmation.ForTimeOfDay,
            Text = confirmation.Text,
            ScheduleRepeatId = confirmation.ScheduleRepeatId,
        };

        if (!confirmation.ImagesGuids.IsNullOrEmpty())
        {
            entity.Files =
                confirmation.ImagesGuids.ConvertAll(g => new Entities.TakingСonfirmationFile { FileGuid = g });
        }

        await _dbContext.TakingСonfirmations.AddAsync(entity, cancellationToken);
        var saved = await _dbContext.TrySaveChangesAsync(_logger, cancellationToken);
        return saved ? entity.ToContractModel() : null;
    }

    public async Task<Contract.TakingСonfirmationPlain?> UpdateConfirmationAsync(Contract.TakingСonfirmationPlain confirmation, TakingСonfirmationUpdateFlags updateFlags, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.TakingСonfirmations
            .Include(c => c.Files)
            .FirstOrDefaultAsync(e => e.Id == confirmation.Id, cancellationToken);

        if (entity == null) return null;

        var entry = _dbContext.Entry(entity);
        entry.UpdateIf(x => x.Text, confirmation.Text, updateFlags.Text);
        entry.UpdateIf(x => x.ForDate, confirmation.ForDate, updateFlags.ForDate);
        entry.UpdateIf(x => x.ForTime, confirmation.ForTime, updateFlags.ForTime);
        entry.UpdateIf(x => x.ForTimeOfDay, confirmation.ForTimeOfDay, updateFlags.ForTimeOfDay);
        entry.UpdateIf(x => x.Text, confirmation.Text, updateFlags.Text);

        if (updateFlags.Images)
        {
            entity.Files.RemoveAndAddExceptExistingByKey(confirmation.ImagesGuids,
                existing => existing.FileGuid, guid => guid,
                guid => new Entities.TakingСonfirmationFile { FileGuid = guid });
        }

        await _dbContext.TakingСonfirmations.AddAsync(entity, cancellationToken);
        var saved = await _dbContext.TrySaveChangesAsync(_logger, cancellationToken);
        return saved ? entity.ToContractModel() : null;
    }

    public async Task<Contract.RemoveOperationResult> RemoveConfirmationAsync(long id, CancellationToken cancellationToken = default)
    {
        var deletedCount = await _dbContext.TakingСonfirmationFiles
            .Where(e => e.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return deletedCount > 0 ? Contract.RemoveOperationResult.Removed : Contract.RemoveOperationResult.NotFound;
    }

    public async Task<Guid?> AddConfirmationImageAsync(long id, Guid fileGuid, CancellationToken cancellationToken = default)
    {
        var entity = new Entities.TakingСonfirmationFile
        {
            TakingСonfirmationId = id,
            FileGuid = default,
        };

        await _dbContext.TakingСonfirmationFiles.AddAsync(entity, cancellationToken);
        var saved = await _dbContext.TrySaveChangesAsync(_logger, cancellationToken);
        return saved ? fileGuid : null;
    }

    public async Task<Contract.RemoveOperationResult> RemoveConfirmationImageAsync(long id, Guid fileGuid, CancellationToken cancellationToken = default)
    {
        var deletedCount = await _dbContext.TakingСonfirmationFiles
            .Where(f => f.TakingСonfirmationId == id && f.FileGuid == fileGuid)
            .ExecuteDeleteAsync(cancellationToken);

        return deletedCount > 0 ? Contract.RemoveOperationResult.Removed : Contract.RemoveOperationResult.NotFound;
    }


    public async Task<List<TakingСonfirmationTimetableTrimmed>> GetTakingConfirmationsForTimetableAsync(List<long> repeatIds,
        CancellationToken cancellationToken = default)
    {
        var takingСonfirmations = await _dbContext.TakingСonfirmations
            .WithFilter(tc => tc.ScheduleRepeatId, repeatIds)
            .Select(EntityMapExpressions.ToScheduleConfirmationTimetable)
            .ToListAsync(cancellationToken);

        return takingСonfirmations;
    }


    public async Task<bool> AnyConfirmationExistsAsync(List<long> repeatIds, CancellationToken cancellationToken = default)
    {
        var exists = await _dbContext.TakingСonfirmations
            .WithFilter(tc => tc.ScheduleRepeatId, repeatIds)
            .AnyAsync(cancellationToken);
        return exists;
    }
}