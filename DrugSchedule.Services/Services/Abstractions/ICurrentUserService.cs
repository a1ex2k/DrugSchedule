using DrugSchedule.BusinessLogic.Models;
using DrugSchedule.BusinessLogic.Utils;
using OneOf.Types;

namespace DrugSchedule.BusinessLogic.Services.Abstractions;

public interface ICurrentUserService
{
    Task<OneOf<True, InvalidInput>> UpdatePasswordAsync(NewPasswordModel newPassword, CancellationToken cancellationToken = default);

    Task<OneOf<UserModel, InvalidInput>> UpdateProfileAsync(UserUpdateModel userUpdateModel, CancellationToken cancellationToken = default);

    Task<UserModel> GetCurrentUserAsync(CancellationToken cancellationToken = default);

    Task<UserContactsCollectionModel> GetUserContactsAsync(CancellationToken cancellationToken = default);

    Task<OneOf<UserPublicCollectionModel, InvalidInput>> FindUsersAsync(UserSearchModel searchModel, CancellationToken cancellationToken = default);

    Task<OneOf<True, InvalidInput, NotFound>> AddUserContactAsync(NewUserContactModel newContactModel, CancellationToken cancellationToken = default);

    Task<OneOf<True, NotFound>> RemoveUserContactAsync(UserIdModel userId, CancellationToken cancellationToken = default);

    Task<OneOf<FileInfoModel, InvalidInput>> SetAvatarAsync(NewFileModel newFileInfoModel, CancellationToken cancellationToken = default);

    Task<OneOf<True, NotFound>> RemoveAvatarAsync(FileInfoRemoveModel fileInfoRemoveModel, CancellationToken cancellationToken = default);
}