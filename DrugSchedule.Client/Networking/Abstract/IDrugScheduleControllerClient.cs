using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Networking;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Client.Networking;

public interface IDrugScheduleControllerClient
{
    Task<ApiCallResult<ScheduleSimpleCollectionDto>> SearchForScheduleAsync(ScheduleSearchDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<ScheduleSimpleDto>> GetScheduleSimpleAsync(ScheduleIdDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<ScheduleSimpleCollectionDto>> GetSchedulesSimpleAsync(TakingScheduleFilterDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<ScheduleExtendedDto>> GetScheduleExtendedAsync(ScheduleIdDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<ScheduleExtendedCollectionDto>> GetSchedulesExtendedAsync(TakingScheduleFilterDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<TakingСonfirmationCollectionDto>> GetTakingConfirmationsAsync(TakingConfirmationFilterDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<ScheduleIdDto>> CreateScheduleAsync(NewScheduleDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<ScheduleIdDto>> UpdateScheduleAsync(ScheduleUpdateDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult> RemoveScheduleAsync(ScheduleIdDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<RepeatIdDto>> CreateRepeatAsync(NewScheduleRepeatDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<RepeatIdDto>> UpdateRepeatAsync(ScheduleRepeatUpdateDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult> RemoveRepeatAsync(RepeatIdDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult> AddOrUpdateShareAsync(ScheduleShareUpdateDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult> RemoveShareAsync(ScheduleShareRemoveDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<ConfirmationIdDto>> CreateConfirmationAsync(NewTakingСonfirmationDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<ConfirmationIdDto>> UpdateConfirmationAsync(TakingСonfirmationUpdateDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult> RemoveConfirmationAsync(ConfirmationIdDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<DownloadableFileDto>> AddConfirmationImageAsync(ConfirmationIdDto confirmationId, UploadFile uploadFile, CancellationToken cancellationToken = default);

    Task<ApiCallResult> RemoveConfirmationImageAsync(ConfirmationImageRemoveDto body, CancellationToken cancellationToken = default);
}