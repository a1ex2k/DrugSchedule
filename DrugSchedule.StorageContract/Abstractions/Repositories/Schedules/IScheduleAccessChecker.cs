using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.StorageContract.Abstractions;

public interface IScheduleAccessChecker
{
    public Task<ScheduleAccessCheck?> GetOwnOrSharedScheduleIdAsync(long scheduleId, long ownerOrShareProfileId, CancellationToken cancellationToken = default);
    
    public Task<List<ScheduleAccessCheck>> GetOwnOrSharedSchedulesIdsAsync(List<long>? scheduleId, long ownerOrShareProfileId, CancellationToken cancellationToken = default);
}