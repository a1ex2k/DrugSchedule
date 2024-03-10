﻿using DrugSchedule.Api.Shared.Dtos;

namespace DrugSchedule.Client.Networking;

public static class UsersControllerClient
{ 
    public static async Task<ApiCallResult<UserFullDto>> GetMeAsync(this IApiClient client, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync<UserFullDto>(EndpointsPaths.User_GetMe, cancellationToken);
    }

    public static async Task<ApiCallResult<UserPublicCollectionDto>> SearchUserAsync(this IApiClient client, UserSearchDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync<UserSearchDto, UserPublicCollectionDto>(body, EndpointsPaths.User_Search, cancellationToken);
    }

    public static async Task<ApiCallResult<UserIdDto>> UpdateProfileAsync(this IApiClient client, UserUpdateDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync<UserUpdateDto, UserIdDto>(body, EndpointsPaths.User_UpdateProfile, cancellationToken);
    }

    public static async Task<ApiCallResult> ChangePasswordAsync(this IApiClient client, NewPasswordDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync(body, EndpointsPaths.User_ChangePassword, cancellationToken);
    }

    public static async Task<ApiCallResult<DownloadableFileDto>> SetAvatarAsync(this IApiClient client, UploadFile uploadFile, CancellationToken cancellationToken = default)
    {
        using var content = new MultipartFormDataContent();
        content.Add(new StreamContent(uploadFile.Stream), "file", uploadFile.Name);
        return await client.PostAsync<DownloadableFileDto>(content, EndpointsPaths.User_SetAvatar, cancellationToken);
    }

    public static async Task<ApiCallResult> RemoveAvatarAsync(this IApiClient client, FileIdDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync(body, EndpointsPaths.User_RemoveAvatar, cancellationToken);
    }
}