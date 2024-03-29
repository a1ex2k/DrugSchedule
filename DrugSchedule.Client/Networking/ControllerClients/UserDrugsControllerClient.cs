using System.Net.Http.Headers;
using System.Text.Json;
using DrugSchedule.Api.Shared.Dtos;

namespace DrugSchedule.Client.Networking;

public static class UserDrugsControllerClient
{
    public static async Task<ApiCallResult<UserMedicamentSimpleDto>> GetSingleUserMedicamentAsync(this IApiClient client, UserMedicamentIdDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync<UserMedicamentIdDto, UserMedicamentSimpleDto>(body, EndpointsPaths.UserDrugs_GetSingle, cancellationToken);
    }

    public static async Task<ApiCallResult<UserMedicamentExtendedDto>> GetSingleExtendedUserMedicamentAsync(this IApiClient client, UserMedicamentIdDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync<UserMedicamentIdDto, UserMedicamentExtendedDto>(body, EndpointsPaths.UserDrugs_GetSingleExtended, cancellationToken);
    }

    public static async Task<ApiCallResult<UserMedicamentSimpleCollectionDto>> GetManyUserMedicamentsAsync(this IApiClient client, UserMedicamentFilterDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync<UserMedicamentFilterDto, UserMedicamentSimpleCollectionDto>(body, EndpointsPaths.UserDrugs_GetMany, cancellationToken);
    }

    public static async Task<ApiCallResult<UserMedicamentExtendedCollectionDto>> GetManyExtendedUserMedicamentsAsync(this IApiClient client, UserMedicamentFilterDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync<UserMedicamentFilterDto, UserMedicamentExtendedCollectionDto>(body, EndpointsPaths.UserDrugs_GetManyExtended, cancellationToken);
    }

    public static async Task<ApiCallResult<UserMedicamentExtendedDto>> GetSharedExtendedUserMedicamentAsync(this IApiClient client, UserMedicamentIdDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync<UserMedicamentIdDto, UserMedicamentExtendedDto>(body, EndpointsPaths.UserDrugs_GetSharedExtended, cancellationToken);
    }

    public static async Task<ApiCallResult<UserMedicamentIdDto>> AddUserMedicamentAsync(this IApiClient client, NewUserMedicamentDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync<NewUserMedicamentDto, UserMedicamentIdDto>(body, EndpointsPaths.UserDrugs_Add, cancellationToken);
    }

    public static async Task<ApiCallResult<DownloadableFileDto>> AddUserMedicamentImage(this IApiClient client, UserMedicamentIdDto userMedicamentId, UploadFile uploadFile, CancellationToken cancellationToken)
    {
        using var content = new MultipartFormDataContent();
        var fileContent = new StreamContent(uploadFile.Stream);
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(uploadFile.ContentType);
        content.Add(fileContent, "file", uploadFile.Name);

        var userMedicamentIdString = JsonSerializer.Serialize(userMedicamentId);
        content.Add(new StringContent(userMedicamentIdString), "userMedicamentId", uploadFile.Name);
        return await client.PostAsync<DownloadableFileDto>(content, EndpointsPaths.UserDrugs_AddImage, cancellationToken);
    }

    public static async Task<ApiCallResult> RemoveUserMedicamentImageAsync(this IApiClient client, UserMedicamentImageRemoveDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync<UserMedicamentImageRemoveDto>(body, EndpointsPaths.UserDrugs_RemoveImage, cancellationToken);
    }

    public static async Task<ApiCallResult<UserMedicamentIdDto>> UpdateUserMedicamentAsync(this IApiClient client, UserMedicamentUpdateDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync<UserMedicamentUpdateDto, UserMedicamentIdDto>(body, EndpointsPaths.UserDrugs_Update, cancellationToken);
    }

    public static async Task<ApiCallResult> RemoveUserMedicamentAsync(this IApiClient client, UserMedicamentIdDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync<UserMedicamentIdDto>(body, EndpointsPaths.UserDrugs_Remove, cancellationToken);
    }
}