using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.StorageContract.Abstractions;

public interface ISharedDataRepository : IScheduleAccessChecker
{
    public Task<TakingScheduleSimple?> GetScheduleSimpleAsync(long id, long userId, CancellationToken cancellationToken = default);

    public Task<List<TakingScheduleSimple>> GetSchedulesSimpleAsync(TakingScheduleFilter filter, long userId, CancellationToken cancellationToken = default);
    
    public Task<List<TakingScheduleSimple>> SearchForOwnedOrSharedAsync(long userId, string searchString, CancellationToken cancellationToken = default);

    public Task<TakingScheduleExtended?> GetScheduleExtendedAsync(long id, long userId, CancellationToken cancellationToken = default);

    public Task<List<TakingScheduleExtended>> GetSchedulesExtendedAsync(TakingScheduleFilter filter, long userId, CancellationToken cancellationToken = default);

    public Task<List<TakingСonfirmation>> GetTakingConfirmationsAsync(TakingConfirmationFilter filter,
        CancellationToken cancellationToken = default);

    public Task<UserMedicamentExtended?> GetSharedUserMedicament(long userMedicamentId, long mustBeSharedWithProfileId, CancellationToken cancellationToken = default);

    public Task<List<long>> GetSharedScheduleIds(long userId, CancellationToken cancellationToken = default);
}