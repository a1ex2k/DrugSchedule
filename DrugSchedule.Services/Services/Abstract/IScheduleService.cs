using DrugSchedule.Services.Models;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Services.Services;

public interface IScheduleService
{
    Task<OneOf<ScheduleSimpleCollection, InvalidInput>> SearchForScheduleAsync(ScheduleSearch searchParams, CancellationToken cancellationToken = default);

    Task<OneOf<ScheduleSimple, NotFound>> GetScheduleSimpleAsync(long scheduleId, CancellationToken cancellationToken = default);

    Task<ScheduleSimpleCollection> GetSchedulesSimpleAsync(TakingScheduleFilter filter, CancellationToken cancellationToken = default);
    
    Task<OneOf<Models.TakingScheduleExtended, NotFound>> GetScheduleExtendedAsync(long scheduleId, CancellationToken cancellationToken = default);
    
    Task<ScheduleExtendedCollection> GetSchedulesExtendedAsync(TakingScheduleFilter filter, CancellationToken cancellationToken = default);
    
    Task<OneOf<TakingСonfirmationCollection, NotFound>> GetTakingConfirmationsAsync(TakingConfirmationFilter filter, CancellationToken cancellationToken = default);

    Task<OneOf<Timetable, NotFound, InvalidInput>> GetScheduleTimetableAsync(long scheduleId, DateOnly minDate, DateOnly maxDate, CancellationToken cancellationToken = default);

    Task<OneOf<Timetable, InvalidInput>> GetTimetableAsync(DateOnly minDate, DateOnly maxDate, CancellationToken cancellationToken = default);
}