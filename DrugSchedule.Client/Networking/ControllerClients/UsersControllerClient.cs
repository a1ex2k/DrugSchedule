using DrugSchedule.Api.Shared.Dtos;

namespace DrugSchedule.Client.Networking;

public class UsersControllerClient : IUsersControllerClient
{
    private readonly IApiClient _client;

    public UsersControllerClient(IApiClient client)
    {
        _client = client;
    }

    public async Task<ApiCallResult<UserFullDto>> GetMeAsync(CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync<UserFullDto>(EndpointsPaths.User_GetMe, cancellationToken);
    }

    public async Task<ApiCallResult<UserPublicCollectionDto>> SearchAsync(UserSearchDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync<UserSearchDto, UserPublicCollectionDto>(body, EndpointsPaths.User_Search, cancellationToken);
    }

    public async Task<ApiCallResult<UserIdDto>> UpdateProfileAsync(UserUpdateDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync<UserUpdateDto, UserIdDto>(body, EndpointsPaths.User_UpdateProfile, cancellationToken);
    }

    public async Task<ApiCallResult> ChangePasswordAsync(NewPasswordDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync(body, EndpointsPaths.User_ChangePassword, cancellationToken);
    }

    public async Task<ApiCallResult<DownloadableFileDto>> SetAvatarAsync(UploadFile uploadFile, CancellationToken cancellationToken = default)
    {
        using var content = new MultipartFormDataContent();
        content.Add(new StreamContent(uploadFile.Stream), "file", uploadFile.Name);
        return await _client.PostAsync<DownloadableFileDto>(content, EndpointsPaths.User_SetAvatar, cancellationToken);
    }

    public async Task<ApiCallResult> RemoveAvatarAsync(FileIdDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync(body, EndpointsPaths.User_RemoveAvatar, cancellationToken);
    }
}