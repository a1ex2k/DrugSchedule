using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DrugSchedule.StorageContract.Data;
using DrugSchedule.StorageContract.Data.Schedule.Plain;

namespace DrugSchedule.StorageContract.Abstractions;

public interface IScheduleConfirmationRepository
{
    public Task<TakingСonfirmationPlain?> GetConfirmationAsync(long id, CancellationToken cancellationToken = default);

    public Task<List<TakingСonfirmationPlain>> GetConfirmationsAsync(long repeatId, CancellationToken cancellationToken = default);

    public Task<TakingСonfirmationPlain?> CreateConfirmationAsync(TakingСonfirmationPlain confirmation, CancellationToken cancellationToken = default);

    public Task<TakingСonfirmationPlain?> UpdateConfirmationAsync(TakingСonfirmationPlain confirmation, CancellationToken cancellationToken = default);

    public Task<RemoveOperationResult> RemoveConfirmationAsync(long id, CancellationToken cancellationToken = default);

    public Task<FileInfo?> AddConfirmationImageAsync(long id, Guid fileGuid, CancellationToken cancellationToken = default);

    public Task<RemoveOperationResult> RemoveConfirmationImageAsync(long id, Guid fileGuid, CancellationToken cancellationToken = default);
}