using DrugSchedule.Services.Models;
using DrugSchedule.Services.Utils;
using DrugSchedule.StorageContract.Data;
using OneOf.Types;

namespace DrugSchedule.Services.Services.Abstractions;

public interface IUserService
{
    Task<OneOf<True, InvalidInput>> UpdatePasswordAsync(NewPasswordModel newPassword, CancellationToken cancellationToken = default);

    Task<OneOf<UserUpdate, InvalidInput>> UpdateProfileAsync(UserUpdate userUpdate, CancellationToken cancellationToken = default);

    Task<UserFull> GetCurrentUserAsync(CancellationToken cancellationToken = default);

    Task<OneOf<DownloadableFile, InvalidInput>> SetAvatarAsync(InputFile inputFileInfo, CancellationToken cancellationToken = default);

    Task<OneOf<True, NotFound>> RemoveAvatarAsync(Guid fileGuid, CancellationToken cancellationToken = default);

    Task<OneOf<UserPublicCollection, InvalidInput>> FindUsersAsync(UserSearch search,
        CancellationToken cancellationToken = default);
}


public interface IScheduleLibraryService
{
    Task<OneOf<ScheduleSimpleCollection, InvalidInput>> SearchForScheduleAsync(string searchString, CancellationToken cancellationToken = default);

    Task<OneOf<TakingScheduleSimple, InvalidInput, NotFound>> GetScheduleSimpleAsync(long id, CancellationToken cancellationToken = default);

    Task<ScheduleSimpleCollection> GetSchedulesSimpleAsync(TakingScheduleFilter filter, CancellationToken cancellationToken = default);
    
    Task<OneOf<Models.TakingScheduleExtended, InvalidInput, NotFound>> GetScheduleExtendedAsync(long id, CancellationToken cancellationToken = default);

    Task<ScheduleExtendedCollection> GetSchedulesExtendedAsync(TakingScheduleFilter filter, CancellationToken cancellationToken = default);

    Task<List<(long Id, long OwnerId)>> GetTakingConfirmationsAsync(long ownerOrShareProfileId, CancellationToken cancellationToken = default);
}