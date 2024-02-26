using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Networking;

namespace DrugSchedule.Client.Networking;

public interface IUserDrugsControllerClient
{
    Task<ApiCallResult<UserMedicamentSimpleDto>> GetSingleAsync(UserMedicamentIdDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<UserMedicamentExtendedDto>> GetSingleExtendedAsync(UserMedicamentIdDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<UserMedicamentSimpleCollectionDto>> GetManyAsync(UserMedicamentFilterDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<UserMedicamentExtendedCollectionDto>> GetManyExtendedAsync(UserMedicamentFilterDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<UserMedicamentExtendedDto>> GetSharedExtendedAsync(UserMedicamentIdDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<UserMedicamentIdDto>> AddAsync(NewUserMedicamentDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<DownloadableFileDto>> AddImage(UserMedicamentIdDto userMedicamentId, UploadFile uploadFile, CancellationToken cancellationToken);

    Task<ApiCallResult> RemoveImageAsync(UserMedicamentImageRemoveDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<UserMedicamentIdDto>> UpdateAsync(UserMedicamentUpdateDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult> RemoveAsync(UserMedicamentIdDto body, CancellationToken cancellationToken = default);
}