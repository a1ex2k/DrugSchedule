using DrugSchedule.Services.Models;
using DrugSchedule.Services.Utils;
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