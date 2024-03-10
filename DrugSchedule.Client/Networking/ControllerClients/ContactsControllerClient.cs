using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Networking;

namespace DrugSchedule.Client.Networking;

public static class ContactsControllerClient
{ 
    public static async Task<ApiCallResult<UserContactsSimpleCollectionDto>> GetAllContactsAsync(this IApiClient client, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync<UserContactsSimpleCollectionDto>(EndpointsPaths.Contacts_GetAll, cancellationToken);
    }

    public static async Task<ApiCallResult<UserContactsSimpleCollectionDto>> GetCommonContactsAsync(this IApiClient client, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync<UserContactsSimpleCollectionDto>(EndpointsPaths.Contacts_GetCommon, cancellationToken);
    }

    public static async Task<ApiCallResult<UserContactDto>> GetSingleExtendedContactAsync(this IApiClient client, UserIdDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync<UserIdDto, UserContactDto>(body, EndpointsPaths.Contacts_GetSingleExtended, cancellationToken);
    }

    public static async Task<ApiCallResult<UserContactsCollectionDto>> GetManyExtendedContactsAsync(this IApiClient client, UserContactFilterDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync<UserContactFilterDto, UserContactsCollectionDto>(body, EndpointsPaths.Contacts_GetManyExtended, cancellationToken);
    }

    public static async Task<ApiCallResult> AddOrUpdateContactAsync(this IApiClient client, NewUserContactDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync(body, EndpointsPaths.Contacts_AddOrUpdate, cancellationToken);
    }

    public static async Task<ApiCallResult> RemoveContactAsync(this IApiClient client, UserIdDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync(body, EndpointsPaths.Contacts_Remove, cancellationToken);
    }
}