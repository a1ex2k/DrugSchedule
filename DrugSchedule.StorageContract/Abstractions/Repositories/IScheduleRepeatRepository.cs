using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.StorageContract.Abstractions;

public interface IScheduleRepeatRepository
{
    public Task<ScheduleRepeatPlain?> GetRepeatAsync(long id, CancellationToken cancellationToken = default);

    public Task<ScheduleRepeatPlain?> GetRepeatAsync(long id, long scheduleId, long userProfileId, CancellationToken cancellationToken = default);
    
    public Task<bool> DoesRepeatExistAsync(long id, long scheduleId, long userProfileId, CancellationToken cancellationToken = default);

    public Task<List<ScheduleRepeatPlain>> GetRepeatsAsync(long scheduleId, CancellationToken cancellationToken = default);

    public Task<ScheduleRepeatPlain?> CreateRepeatAsync(ScheduleRepeatPlain repeat, CancellationToken cancellationToken = default);

    public Task<ScheduleRepeatPlain?> UpdateRepeatAsync(ScheduleRepeatPlain repeat, ScheduleRepeatUpdateFlags updateFlags, CancellationToken cancellationToken = default);

    public Task<RemoveOperationResult> RemoveRepeatAsync(long id, CancellationToken cancellationToken = default);
}