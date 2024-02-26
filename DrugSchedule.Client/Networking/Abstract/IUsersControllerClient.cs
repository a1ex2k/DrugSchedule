using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Networking;

namespace DrugSchedule.Client.Networking;

public interface IUsersControllerClient
{
    Task<ApiCallResult<UserFullDto>> GetMeAsync(CancellationToken cancellationToken = default);

    Task<ApiCallResult<UserPublicCollectionDto>> SearchAsync(UserSearchDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<UserIdDto>> UpdateProfileAsync(UserUpdateDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult> ChangePasswordAsync(NewPasswordDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<DownloadableFileDto>> SetAvatarAsync(UploadFile uploadFile, CancellationToken cancellationToken = default);

    Task<ApiCallResult> RemoveAvatarAsync(FileIdDto body, CancellationToken cancellationToken = default);
}