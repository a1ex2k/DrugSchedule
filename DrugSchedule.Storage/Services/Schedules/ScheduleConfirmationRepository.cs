﻿using DrugSchedule.Storage.Data;
using DrugSchedule.Storage.Extensions;
using DrugSchedule.StorageContract.Abstractions;
using DrugSchedule.StorageContract.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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

    public async Task<List<Contract.TakingСonfirmationPlain>> GetConfirmationsAsync(long repeatId, CancellationToken cancellationToken = default)
    {
        var confirmations = await _dbContext.TakingСonfirmations
            .Where(s => s.ScheduleRepeatId == repeatId)
            .Select(EntityMapExpressions.ToScheduleConfirmationPlain)
            .ToListAsync(cancellationToken);
        return confirmations;
    }

    public async Task<bool> DoesConfirmationExistAsync(long id, long repeatId, long scheduleId, CancellationToken cancellationToken = default)
    {
        var exists = await _dbContext.TakingСonfirmations
            .AsNoTracking()
            .Where(s => s.Id == id && s.ScheduleRepeatId == repeatId)
            .Where(s => s.ScheduleRepeat!.MedicamentTakingScheduleId == scheduleId)
            .AnyAsync(cancellationToken);
        return exists;
    }

    public async Task<Contract.TakingСonfirmationPlain?> CreateConfirmationAsync(Contract.TakingСonfirmationPlain confirmation, CancellationToken cancellationToken = default)
    {
        var entity = new Entities.TakingСonfirmation
        {
            CreatedAt = confirmation.CreatedAt,
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

        if (updateFlags.Text)
        {
            entity.Text = confirmation.Text;
        }

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
}