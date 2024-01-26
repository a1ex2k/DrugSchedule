using DrugSchedule.BusinessLogic.Models;
using DrugSchedule.BusinessLogic.Utils;
using OneOf.Types;

namespace DrugSchedule.BusinessLogic.Services.Abstractions;

public interface IUserService
{
    Task<OneOf<True, InvalidInput>> UpdatePasswordAsync(NewPasswordModel newPassword, CancellationToken cancellationToken = default);

    Task<OneOf<UserFullModel, InvalidInput>> UpdateProfileAsync(UserUpdateModel userUpdateModel, CancellationToken cancellationToken = default);

    Task<UserFullModel> GetCurrentUserAsync(CancellationToken cancellationToken = default);

    Task<OneOf<FileInfoModel, InvalidInput>> SetAvatarAsync(NewFile newFileInfo, CancellationToken cancellationToken = default);

    Task<OneOf<True, NotFound>> RemoveAvatarAsync(FileInfoRemoveModel fileInfoRemoveModel, CancellationToken cancellationToken = default);

    Task<FileInfoCollectionModel> GetAvatarsInfoAsync(FileInfoRequestModel fileInfoRemoveModel, CancellationToken cancellationToken = default);

    Task<OneOf<UserPublicCollectionModel, InvalidInput>> FindUsersAsync(UserSearchModel searchModel,
        CancellationToken cancellationToken = default);
}