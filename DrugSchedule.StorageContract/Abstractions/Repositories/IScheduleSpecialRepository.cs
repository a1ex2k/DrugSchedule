using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.StorageContract.Abstractions;

public interface IScheduleSpecialRepository
{
    public Task<TakingScheduleSimple?> GetScheduleSimpleAsync(long ownerOrShareProfileId, long id, CancellationToken cancellationToken = default);

    public Task<List<TakingScheduleSimple>> GetSchedulesSimpleAsync(long ownerOrShareProfileId, TakingScheduleFilter filter, CancellationToken cancellationToken = default);
    
    public Task<List<TakingScheduleSimple>> SearchForOwnedOrSharedAsync(long ownerOrShareProfileId, string searchString, CancellationToken cancellationToken = default);

    public Task<TakingScheduleExtended?> GetScheduleExtendedAsync(long ownerOrShareProfileId, long id, CancellationToken cancellationToken = default);

    public Task<List<TakingScheduleExtended>> GetSchedulesExtendedAsync(long ownerOrShareProfileId, TakingScheduleFilter filter, CancellationToken cancellationToken = default);
    
    public Task<List<ScheduleShareExtended>> GetScheduleSharesExtendedAsync(long ownerOrShareProfileId, TakingConfirmationFilter filter, CancellationToken cancellationToken = default);
    
    public Task<List<(long Id, long OwnerId)>> GetOwnOrSharedSchedulesIdsAsync(long ownerOrShareProfileId, CancellationToken cancellationToken = default);
    
    public Task<long> GetOwnerUserIdAsync(long scheduleId, CancellationToken cancellationToken = default);
}