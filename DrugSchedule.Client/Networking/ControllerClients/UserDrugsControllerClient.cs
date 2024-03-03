using System.Text.Json;
using DrugSchedule.Api.Shared.Dtos;

namespace DrugSchedule.Client.Networking;

public class UserDrugsControllerClient : IUserDrugsControllerClient
{
    private readonly IApiClient _client;

    public UserDrugsControllerClient(IApiClient client)
    {
        _client = client;
    }

    public async Task<ApiCallResult<UserMedicamentSimpleDto>> GetSingleAsync(UserMedicamentIdDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync<UserMedicamentIdDto, UserMedicamentSimpleDto>(body, EndpointsPaths.UserDrugs_GetSingle, cancellationToken);
    }

    public async Task<ApiCallResult<UserMedicamentExtendedDto>> GetSingleExtendedAsync(UserMedicamentIdDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync<UserMedicamentIdDto, UserMedicamentExtendedDto>(body, EndpointsPaths.UserDrugs_GetSingleExtended, cancellationToken);
    }

    public async Task<ApiCallResult<UserMedicamentSimpleCollectionDto>> GetManyAsync(UserMedicamentFilterDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync<UserMedicamentFilterDto, UserMedicamentSimpleCollectionDto>(body, EndpointsPaths.UserDrugs_GetMany, cancellationToken);
    }

    public async Task<ApiCallResult<UserMedicamentExtendedCollectionDto>> GetManyExtendedAsync(UserMedicamentFilterDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync<UserMedicamentFilterDto, UserMedicamentExtendedCollectionDto>(body, EndpointsPaths.UserDrugs_GetManyExtended, cancellationToken);
    }

    public async Task<ApiCallResult<UserMedicamentExtendedDto>> GetSharedExtendedAsync(UserMedicamentIdDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync<UserMedicamentIdDto, UserMedicamentExtendedDto>(body, EndpointsPaths.UserDrugs_GetSharedExtended, cancellationToken);
    }

    public async Task<ApiCallResult<UserMedicamentIdDto>> AddAsync(NewUserMedicamentDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync<NewUserMedicamentDto, UserMedicamentIdDto>(body, EndpointsPaths.UserDrugs_Add, cancellationToken);
    }

    public async Task<ApiCallResult<DownloadableFileDto>> AddImage(UserMedicamentIdDto userMedicamentId, UploadFile uploadFile, CancellationToken cancellationToken)
    {
        using var content = new MultipartFormDataContent();
        content.Add(new StreamContent(uploadFile.Stream), "file", uploadFile.Name);
        var userMedicamentIdString = JsonSerializer.Serialize(userMedicamentId);
        content.Add(new StringContent(userMedicamentIdString), "userMedicamentId", uploadFile.Name);
        return await _client.PostAsync<DownloadableFileDto>(content, EndpointsPaths.UserDrugs_AddImage, cancellationToken);
    }

    public async Task<ApiCallResult> RemoveImageAsync(UserMedicamentImageRemoveDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync<UserMedicamentImageRemoveDto>(body, EndpointsPaths.UserDrugs_RemoveImage, cancellationToken);
    }

    public async Task<ApiCallResult<UserMedicamentIdDto>> UpdateAsync(UserMedicamentUpdateDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync<UserMedicamentUpdateDto, UserMedicamentIdDto>(body, EndpointsPaths.UserDrugs_Update, cancellationToken);
    }

    public async Task<ApiCallResult> RemoveAsync(UserMedicamentIdDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync<UserMedicamentIdDto>(body, EndpointsPaths.UserDrugs_Remove, cancellationToken);
    }
}