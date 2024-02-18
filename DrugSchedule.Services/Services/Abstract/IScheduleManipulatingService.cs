using DrugSchedule.Services.Models;
using OneOf.Types;

namespace DrugSchedule.Services.Services;

public interface IScheduleManipulatingService
{
    Task<OneOf<ScheduleId, NotFound, InvalidInput>> CreateScheduleAsync(NewSchedule newSchedule, CancellationToken cancellationToken = default);

    Task<OneOf<ScheduleId, NotFound, InvalidInput>> UpdateScheduleAsync(ScheduleUpdate update, CancellationToken cancellationToken = default);

    Task<OneOf<True, NotFound>> RemoveSchedule(long scheduleId, CancellationToken cancellationToken = default);


    Task<OneOf<RepeatId, NotFound, InvalidInput>> CreateRepeatAsync(NewScheduleRepeat newRepeat, CancellationToken cancellationToken = default);

    Task<OneOf<RepeatId, NotFound, InvalidInput>> UpdateRepeatAsync(ScheduleRepeatUpdate repeatUpdate, CancellationToken cancellationToken = default);

    Task<OneOf<True, NotFound>> RemoveRepeatAsync(long repeatId, CancellationToken cancellationToken = default);


    Task<OneOf<True, NotFound, InvalidInput>> AddOrUpdateShareAsync(ScheduleShareUpdate newShare, CancellationToken cancellationToken = default);

    Task<OneOf<True, NotFound>> RemoveShareAsync(long scheduleId, long contactProfileId, CancellationToken cancellationToken = default);
}