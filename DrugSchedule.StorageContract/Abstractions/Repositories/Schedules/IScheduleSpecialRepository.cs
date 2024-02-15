using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.StorageContract.Abstractions;

public interface IScheduleSpecialRepository : IScheduleAccessChecker
{
    public Task<TakingScheduleSimple?> GetScheduleSimpleAsync(long id, long ownerOrShareProfileId, CancellationToken cancellationToken = default);

    public Task<List<TakingScheduleSimple>> GetSchedulesSimpleAsync(TakingScheduleFilter filter, long ownerOrShareProfileId, CancellationToken cancellationToken = default);
    
    public Task<List<TakingScheduleSimple>> SearchForOwnedOrSharedAsync(long ownerOrShareProfileId, string searchString, CancellationToken cancellationToken = default);

    public Task<TakingScheduleExtended?> GetScheduleExtendedAsync(long id, long ownerOrShareProfileId, CancellationToken cancellationToken = default);

    public Task<List<TakingScheduleExtended>> GetSchedulesExtendedAsync(TakingScheduleFilter filter, long ownerOrShareProfileId, CancellationToken cancellationToken = default);
}