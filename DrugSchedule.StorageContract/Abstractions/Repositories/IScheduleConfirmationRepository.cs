using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.StorageContract.Abstractions;

public interface IScheduleConfirmationRepository
{
    public Task<TakingСonfirmationPlain?> GetConfirmationAsync(long id, CancellationToken cancellationToken = default);

    public Task<List<TakingСonfirmationPlain>> GetConfirmationsAsync(long repeatId, CancellationToken cancellationToken = default);

    public Task<bool> DoesConfirmationExistAsync(long confirmationId, long repeatId, long scheduleId, CancellationToken cancellationToken = default);

    public Task<TakingСonfirmationPlain?> CreateConfirmationAsync(TakingСonfirmationPlain confirmation, CancellationToken cancellationToken = default);

    public Task<TakingСonfirmationPlain?> UpdateConfirmationAsync(TakingСonfirmationPlain confirmation, TakingСonfirmationUpdateFlags updateFlags, CancellationToken cancellationToken = default);

    public Task<RemoveOperationResult> RemoveConfirmationAsync(long id, CancellationToken cancellationToken = default);

    public Task<Guid?> AddConfirmationImageAsync(long id, Guid fileGuid, CancellationToken cancellationToken = default);

    public Task<RemoveOperationResult> RemoveConfirmationImageAsync(long id, Guid fileGuid, CancellationToken cancellationToken = default);
}