using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Client.Networking;

public static class DrugScheduleControllerClient
{
    public static async Task<ApiCallResult<ScheduleSimpleCollectionDto>> SearchForScheduleAsync(this IApiClient client, ScheduleSearchDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync<ScheduleSearchDto, ScheduleSimpleCollectionDto>(body, EndpointsPaths.Schedule_SearchForSchedule, cancellationToken);
    }

    public static async Task<ApiCallResult<ScheduleSimpleDto>> GetScheduleSimpleAsync(this IApiClient client, ScheduleIdDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync<ScheduleIdDto, ScheduleSimpleDto>(body, EndpointsPaths.Schedule_GetScheduleSimple, cancellationToken);
    }

    public static async Task<ApiCallResult<ScheduleSimpleCollectionDto>> GetSchedulesSimpleAsync(this IApiClient client, TakingScheduleFilterDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync<TakingScheduleFilterDto, ScheduleSimpleCollectionDto>(body, EndpointsPaths.Schedule_GetSchedulesSimple, cancellationToken);
    }

    public static async Task<ApiCallResult<ScheduleExtendedDto>> GetScheduleExtendedAsync(this IApiClient client, ScheduleIdDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync<ScheduleIdDto, ScheduleExtendedDto>(body, EndpointsPaths.Schedule_GetScheduleExtended, cancellationToken);
    }

    public static async Task<ApiCallResult<ScheduleExtendedCollectionDto>> GetSchedulesExtendedAsync(this IApiClient client, TakingScheduleFilterDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync<TakingScheduleFilterDto, ScheduleExtendedCollectionDto>(body, EndpointsPaths.Schedule_GetSchedulesExtended, cancellationToken);
    }

    public static async Task<ApiCallResult<TakingСonfirmationCollectionDto>> GetTakingConfirmationsAsync(this IApiClient client, TakingConfirmationFilterDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync<TakingConfirmationFilterDto, TakingСonfirmationCollectionDto>(body, EndpointsPaths.Schedule_GetTakingConfirmations, cancellationToken);
    }

    public static async Task<ApiCallResult<ScheduleIdDto>> CreateScheduleAsync(this IApiClient client, NewScheduleDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync<NewScheduleDto, ScheduleIdDto>(body, EndpointsPaths.Schedule_CreateSchedule, cancellationToken);
    }

    public static async Task<ApiCallResult<ScheduleIdDto>> UpdateScheduleAsync(this IApiClient client, ScheduleUpdateDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync<ScheduleUpdateDto, ScheduleIdDto>(body, EndpointsPaths.Schedule_UpdateSchedule, cancellationToken);
    }

    public static async Task<ApiCallResult> RemoveScheduleAsync(this IApiClient client, ScheduleIdDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync(body, EndpointsPaths.Schedule_RemoveSchedule, cancellationToken);
    }

    public static async Task<ApiCallResult<RepeatIdDto>> CreateRepeatAsync(this IApiClient client, NewScheduleRepeatDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync<NewScheduleRepeatDto, RepeatIdDto>(body, EndpointsPaths.Schedule_CreateRepeat, cancellationToken);
    }

    public static async Task<ApiCallResult<RepeatIdDto>> UpdateRepeatAsync(this IApiClient client, ScheduleRepeatUpdateDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync<ScheduleRepeatUpdateDto, RepeatIdDto>(body, EndpointsPaths.Schedule_UpdateRepeat, cancellationToken);
    }

    public static async Task<ApiCallResult> RemoveRepeatAsync(this IApiClient client, RepeatIdDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync(body, EndpointsPaths.Schedule_RemoveRepeat, cancellationToken);
    }

    public static async Task<ApiCallResult> AddOrUpdateShareAsync(this IApiClient client, ScheduleShareUpdateDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync(body, EndpointsPaths.Schedule_AddOrUpdateShare, cancellationToken);
    }

    public static async Task<ApiCallResult> RemoveShareAsync(this IApiClient client, ScheduleShareRemoveDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync(body, EndpointsPaths.Schedule_RemoveShare, cancellationToken);
    }

    public static async Task<ApiCallResult<ConfirmationIdDto>> CreateConfirmationAsync(this IApiClient client, NewTakingСonfirmationDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync<NewTakingСonfirmationDto, ConfirmationIdDto>(body, EndpointsPaths.Schedule_CreateConfirmation, cancellationToken);
    }

    public static async Task<ApiCallResult<ConfirmationIdDto>> UpdateConfirmationAsync(this IApiClient client, TakingСonfirmationUpdateDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync<TakingСonfirmationUpdateDto, ConfirmationIdDto>(body, EndpointsPaths.Schedule_UpdateConfirmation, cancellationToken);
    }

    public static async Task<ApiCallResult> RemoveConfirmationAsync(this IApiClient client, ConfirmationIdDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync(body, EndpointsPaths.Schedule_RemoveConfirmation, cancellationToken);
    }

    public static async Task<ApiCallResult<DownloadableFileDto>> AddConfirmationImageAsync(this IApiClient client, ConfirmationIdDto confirmationId, UploadFile uploadFile, CancellationToken cancellationToken = default)
    {
        using var content = new MultipartFormDataContent();

        var fileContent = new StreamContent(uploadFile.Stream);
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(uploadFile.ContentType);
        content.Add(fileContent, "file", uploadFile.Name);

        content.Add(JsonContent.Create(confirmationId), "confirmationIdDtoJson");

        return await client.PostAsync<DownloadableFileDto>(content, EndpointsPaths.Schedule_AddConfirmationImage, cancellationToken);
    }

    public static async Task<ApiCallResult> RemoveConfirmationImageAsync(this IApiClient client, ConfirmationImageRemoveDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync(body, EndpointsPaths.Schedule_RemoveConfirmationImage, cancellationToken);
    }  
    
    public static async Task<ApiCallResult<TimetableDto>> GetTimetableAsync(this IApiClient client, TimetableFilterDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync<TimetableFilterDto, TimetableDto>(body, EndpointsPaths.Schedule_GetTimetable, cancellationToken);
    }
}