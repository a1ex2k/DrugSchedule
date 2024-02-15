using DrugSchedule.Services.Errors;
using DrugSchedule.Services.Models;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Services.Services;

public interface IScheduleReadService
{
    Task<OneOf<ScheduleSimpleCollection, InvalidInput>> SearchForScheduleAsync(string searchString, CancellationToken cancellationToken = default);

    Task<OneOf<ScheduleSimple, NotFound>> GetScheduleSimpleAsync(long scheduleId, CancellationToken cancellationToken = default);

    Task<ScheduleSimpleCollection> GetSchedulesSimpleAsync(TakingScheduleFilter filter, CancellationToken cancellationToken = default);
    
    Task<OneOf<Models.TakingScheduleExtended, NotFound>> GetScheduleExtendedAsync(long scheduleId, CancellationToken cancellationToken = default);
    
    Task<ScheduleExtendedCollection> GetSchedulesExtendedAsync(TakingScheduleFilter filter, CancellationToken cancellationToken = default);
    
    Task<OneOf<TakingСonfirmationCollection, NotFound>> GetTakingConfirmationsAsync(long confirmationId, long scheduleId, CancellationToken cancellationToken = default);

    Task<TakingСonfirmationCollection> GetTakingConfirmationsAsync(TakingConfirmationFilter filter, CancellationToken cancellationToken = default);
}