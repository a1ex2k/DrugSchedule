using DrugSchedule.BusinessLogic.Models;
using DrugSchedule.BusinessLogic.Utils;
using OneOf.Types;

namespace DrugSchedule.BusinessLogic.Services.Abstractions;

public interface IUserService
{
    Task<OneOf<True, InvalidInput>> UpdatePasswordAsync(NewPasswordModel newPassword, CancellationToken cancellationToken = default);

    Task<OneOf<UserUpdate, InvalidInput>> UpdateProfileAsync(UserUpdate userUpdate, CancellationToken cancellationToken = default);

    Task<UserFull> GetCurrentUserAsync(CancellationToken cancellationToken = default);

    Task<OneOf<DownloadableFile, InvalidInput>> SetAvatarAsync(InputFile inputFileInfo, CancellationToken cancellationToken = default);

    Task<OneOf<True, NotFound>> RemoveAvatarAsync(FileId fileId, CancellationToken cancellationToken = default);

    Task<OneOf<UserPublicCollection, InvalidInput>> FindUsersAsync(UserSearch search,
        CancellationToken cancellationToken = default);
}