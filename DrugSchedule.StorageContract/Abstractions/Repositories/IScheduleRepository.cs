using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.StorageContract.Abstractions;

public interface IScheduleRepository
{
    public Task<TakingSchedulePlain?> GetTakingScheduleAsync(long id, CancellationToken cancellationToken = default);

    public Task<TakingSchedulePlain?> GetTakingScheduleAsync(long id, long userId, CancellationToken cancellationToken = default);

    public Task<bool> DoesScheduleExistsAsync(long id, long userId, CancellationToken cancellationToken = default);

    public Task<List<TakingSchedulePlain>> GetTakingSchedulesAsync(TakingScheduleFilter filter, long userId, CancellationToken cancellationToken = default);

    public Task<TakingSchedulePlain?> CreateTakingScheduleAsync(TakingSchedulePlain takingSchedule, CancellationToken cancellationToken = default);

    public Task<TakingSchedulePlain?> UpdateTakingScheduleAsync(TakingSchedulePlain takingSchedule, TakingScheduleUpdateFlags updateFlags, CancellationToken cancellationToken = default);
  
    public Task<RemoveOperationResult> RemoveTakingScheduleAsync(long id, long userId, CancellationToken cancellationToken = default);
}