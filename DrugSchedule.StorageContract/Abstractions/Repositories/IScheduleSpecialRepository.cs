using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.StorageContract.Abstractions;

public interface IScheduleSpecialRepository
{
    public Task<TakingScheduleSimple?> GetScheduleSimpleAsync(long id, CancellationToken cancellationToken = default);

    public Task<List<TakingScheduleSimple>> GetSchedulesSimpleAsync(TakingScheduleFilter filter, CancellationToken cancellationToken = default);
    
    public Task<List<TakingScheduleSimple>> GetScheduleSimpleAsync(long userProfileId, string searchString, CancellationToken cancellationToken = default);

    public Task<TakingScheduleExtended?> GetScheduleExtendedAsync(long id, CancellationToken cancellationToken = default);

    public Task<List<TakingScheduleExtended>> GetScheduleExtendedAsync(TakingScheduleFilter filter, CancellationToken cancellationToken = default);
}