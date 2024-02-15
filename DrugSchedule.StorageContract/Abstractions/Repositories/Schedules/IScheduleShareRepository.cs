using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.StorageContract.Abstractions;

public interface IScheduleShareRepository
{
    public Task<ScheduleSharePlain?> GetScheduleShareAsync(long id, CancellationToken cancellationToken = default);

    public Task<List<ScheduleSharePlain>> GetScheduleSharesAsync(long scheduleId, CancellationToken cancellationToken = default);

    public Task<ScheduleSharePlain?> AddOrUpdateShareAsync(ScheduleSharePlain scheduleShare, CancellationToken cancellationToken = default);

    public Task<RemoveOperationResult> RemoveTakingScheduleAsync(long id, CancellationToken cancellationToken = default);
}