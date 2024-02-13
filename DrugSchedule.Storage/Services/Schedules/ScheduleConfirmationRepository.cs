using DrugSchedule.Storage.Data;
using DrugSchedule.StorageContract.Abstractions;
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
        throw new NotImplementedException();
    }

    public async Task<List<Contract.TakingСonfirmationPlain>> GetConfirmationsAsync(long repeatId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<Contract.TakingСonfirmationPlain?> CreateConfirmationAsync(Contract.TakingСonfirmationPlain confirmation, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<Contract.TakingСonfirmationPlain?> UpdateConfirmationAsync(Contract.TakingСonfirmationPlain confirmation, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<Contract.RemoveOperationResult> RemoveConfirmationAsync(long id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<Contract.FileInfo?> AddConfirmationImageAsync(long id, Guid fileGuid, CancellationToken cancellationToken = default)
    {
        var entity
    }

    public async Task<Contract.RemoveOperationResult> RemoveConfirmationImageAsync(long id, Guid fileGuid, CancellationToken cancellationToken = default)
    {
        var deletedCount = await _dbContext.TakingСonfirmationFiles
            .Where(f => f.TakingСonfirmationId == id && f.FileGuid == fileGuid)
            .ExecuteDeleteAsync(cancellationToken);

        return deletedCount > 0 ? Contract.RemoveOperationResult.Removed : Contract.RemoveOperationResult.NotFound;
    }
}