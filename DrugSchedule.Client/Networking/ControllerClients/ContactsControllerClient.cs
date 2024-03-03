using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Networking;

namespace DrugSchedule.Client.Networking;

public class ContactsControllerClient : IContactsControllerClient
{
    private readonly IApiClient _client;

    public ContactsControllerClient(IApiClient client)
    {
        _client = client;
    }

    public async Task<ApiCallResult<UserContactsSimpleCollectionDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync<UserContactsSimpleCollectionDto>(EndpointsPaths.Contacts_GetAll, cancellationToken);
    }

    public async Task<ApiCallResult<UserContactsSimpleCollectionDto>> GetCommonAsync(CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync<UserContactsSimpleCollectionDto>(EndpointsPaths.Contacts_GetCommon, cancellationToken);
    }

    public async Task<ApiCallResult<UserContactDto>> GetSingleExtendedAsync(UserIdDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync<UserIdDto, UserContactDto>(body, EndpointsPaths.Contacts_GetSingleExtended, cancellationToken);
    }

    public async Task<ApiCallResult<UserContactsCollectionDto>> GetManyExtendedAsync(UserContactFilterDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync<UserContactFilterDto, UserContactsCollectionDto>(body, EndpointsPaths.Contacts_GetManyExtended, cancellationToken);
    }

    public async Task<ApiCallResult> AddOrUpdateAsync(NewUserContactDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync(body, EndpointsPaths.Contacts_AddOrUpdate, cancellationToken);
    }

    public async Task<ApiCallResult> RemoveAsync(UserIdDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync(body, EndpointsPaths.Contacts_Remove, cancellationToken);
    }
}