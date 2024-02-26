using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Networking;

namespace DrugSchedule.Client.Networking;

public interface IContactsControllerClient
{
    Task<ApiCallResult<UserContactsSimpleCollectionDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<ApiCallResult<UserContactsSimpleCollectionDto>> GetCommonAsync(CancellationToken cancellationToken = default);

    Task<ApiCallResult<UserContactDto>> GetSingleExtendedAsync(UserIdDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<UserContactsCollectionDto>> GetManyExtendedAsync(UserContactFilterDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult> AddOrUpdateAsync(NewUserContactDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult> RemoveAsync(UserIdDto body, CancellationToken cancellationToken = default);
}