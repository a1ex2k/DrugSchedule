using System.Text.Json;
using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Client.Networking;

public class DrugScheduleControllerClient : IDrugScheduleControllerClient
{
    private readonly IApiClient _client;

    public DrugScheduleControllerClient(IApiClient client)
    {
        _client = client;
    }

    public async Task<ApiCallResult<ScheduleSimpleCollectionDto>> SearchForScheduleAsync(ScheduleSearchDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync<ScheduleSearchDto, ScheduleSimpleCollectionDto>(body, EndpointsPaths.Schedule_SearchForSchedule, cancellationToken);
    }

    public async Task<ApiCallResult<ScheduleSimpleDto>> GetScheduleSimpleAsync(ScheduleIdDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync<ScheduleIdDto, ScheduleSimpleDto>(body, EndpointsPaths.Schedule_GetScheduleSimple, cancellationToken);
    }

    public async Task<ApiCallResult<ScheduleSimpleCollectionDto>> GetSchedulesSimpleAsync(TakingScheduleFilterDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync<TakingScheduleFilterDto, ScheduleSimpleCollectionDto>(body, EndpointsPaths.Schedule_GetSchedulesSimple, cancellationToken);
    }

    public async Task<ApiCallResult<ScheduleExtendedDto>> GetScheduleExtendedAsync(ScheduleIdDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync<ScheduleIdDto, ScheduleExtendedDto>(body, EndpointsPaths.Schedule_GetScheduleExtended, cancellationToken);
    }

    public async Task<ApiCallResult<ScheduleExtendedCollectionDto>> GetSchedulesExtendedAsync(TakingScheduleFilterDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync<TakingScheduleFilterDto, ScheduleExtendedCollectionDto>(body, EndpointsPaths.Schedule_GetSchedulesExtended, cancellationToken);
    }

    public async Task<ApiCallResult<TakingСonfirmationCollectionDto>> GetTakingConfirmationsAsync(TakingConfirmationFilterDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync<TakingConfirmationFilterDto, TakingСonfirmationCollectionDto>(body, EndpointsPaths.Schedule_GetTakingConfirmations, cancellationToken);
    }

    public async Task<ApiCallResult<ScheduleIdDto>> CreateScheduleAsync(NewScheduleDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync<NewScheduleDto, ScheduleIdDto>(body, EndpointsPaths.Schedule_CreateSchedule, cancellationToken);
    }

    public async Task<ApiCallResult<ScheduleIdDto>> UpdateScheduleAsync(ScheduleUpdateDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync<ScheduleUpdateDto, ScheduleIdDto>(body, EndpointsPaths.Schedule_UpdateSchedule, cancellationToken);
    }

    public async Task<ApiCallResult> RemoveScheduleAsync(ScheduleIdDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync(body, EndpointsPaths.Schedule_RemoveSchedule, cancellationToken);
    }

    public async Task<ApiCallResult<RepeatIdDto>> CreateRepeatAsync(NewScheduleRepeatDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync<NewScheduleRepeatDto, RepeatIdDto>(body, EndpointsPaths.Schedule_CreateRepeat, cancellationToken);
    }

    public async Task<ApiCallResult<RepeatIdDto>> UpdateRepeatAsync(ScheduleRepeatUpdateDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync<ScheduleRepeatUpdateDto, RepeatIdDto>(body, EndpointsPaths.Schedule_UpdateRepeat, cancellationToken);
    }

    public async Task<ApiCallResult> RemoveRepeatAsync(RepeatIdDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync(body, EndpointsPaths.Schedule_RemoveRepeat, cancellationToken);
    }

    public async Task<ApiCallResult> AddOrUpdateShareAsync(ScheduleShareUpdateDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync(body, EndpointsPaths.Schedule_AddOrUpdateShare, cancellationToken);
    }

    public async Task<ApiCallResult> RemoveShareAsync(ScheduleShareRemoveDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync(body, EndpointsPaths.Schedule_RemoveShare, cancellationToken);
    }

    public async Task<ApiCallResult<ConfirmationIdDto>> CreateConfirmationAsync(NewTakingСonfirmationDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync<NewTakingСonfirmationDto, ConfirmationIdDto>(body, EndpointsPaths.Schedule_CreateConfirmation, cancellationToken);
    }

    public async Task<ApiCallResult<ConfirmationIdDto>> UpdateConfirmationAsync(TakingСonfirmationUpdateDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync<TakingСonfirmationUpdateDto, ConfirmationIdDto>(body, EndpointsPaths.Schedule_UpdateConfirmation, cancellationToken);
    }

    public async Task<ApiCallResult> RemoveConfirmationAsync(ConfirmationIdDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync(body, EndpointsPaths.Schedule_RemoveConfirmation, cancellationToken);
    }

    public async Task<ApiCallResult<DownloadableFileDto>> AddConfirmationImageAsync(ConfirmationIdDto confirmationId, UploadFile uploadFile, CancellationToken cancellationToken = default)
    {
        using var content = new MultipartFormDataContent();
        content.Add(new StreamContent(uploadFile.Stream), "file", uploadFile.Name);
        var confirmationIdString = JsonSerializer.Serialize(confirmationId);
        content.Add(new StringContent(confirmationIdString), "confirmationId", uploadFile.Name);
        return await _client.PostAsync<DownloadableFileDto>(content, EndpointsPaths.Schedule_AddConfirmationImage, cancellationToken);
    }

    public async Task<ApiCallResult> RemoveConfirmationImageAsync(ConfirmationImageRemoveDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync(body, EndpointsPaths.Schedule_RemoveConfirmation, cancellationToken);
    }
}