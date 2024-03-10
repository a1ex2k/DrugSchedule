using DrugSchedule.Api.Shared.Dtos;

namespace DrugSchedule.Client.Networking;

public static class DrugLibraryControllerClient 
{
    public static async Task<ApiCallResult<MedicamentSimpleDto>> GetMedicamentAsync(this IApiClient client, MedicamentIdDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync<MedicamentIdDto, MedicamentSimpleDto>(body, EndpointsPaths.DrugLibrary_GetMedicament, cancellationToken);
    }

    public static async Task<ApiCallResult<MedicamentExtendedDto>> GetMedicamentExtendedAsync(this IApiClient client, MedicamentIdDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync<MedicamentIdDto, MedicamentExtendedDto>(body, EndpointsPaths.DrugLibrary_GetMedicamentExtended, cancellationToken);
    }

    public static async Task<ApiCallResult<MedicamentSimpleCollectionDto>> GetMedicamentsAsync(this IApiClient client, MedicamentFilterDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync<MedicamentFilterDto, MedicamentSimpleCollectionDto>(body, EndpointsPaths.DrugLibrary_GetMedicaments, cancellationToken);
    }

    public static async Task<ApiCallResult<MedicamentExtendedCollectionDto>> GetMedicamentsExtendedAsync(this IApiClient client, MedicamentFilterDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync<MedicamentFilterDto, MedicamentExtendedCollectionDto>(body, EndpointsPaths.DrugLibrary_GetMedicamentsExtended, cancellationToken);
    }

    public static async Task<ApiCallResult<ManufacturerDto>> GetManufacturerAsync(this IApiClient client, ManufacturerIdDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync<ManufacturerIdDto, ManufacturerDto>(body, EndpointsPaths.DrugLibrary_GetManufacturer, cancellationToken);
    }

    public static async Task<ApiCallResult<ManufacturerCollectionDto>> GetManufacturersAsync(this IApiClient client, ManufacturerFilterDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync<ManufacturerFilterDto, ManufacturerCollectionDto>(body, EndpointsPaths.DrugLibrary_GetManufacturers, cancellationToken);
    }

    public static async Task<ApiCallResult<ReleaseFormCollectionDto>> GetReleaseFormsAsync(this IApiClient client, MedicamentReleaseFormFilterDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync<MedicamentReleaseFormFilterDto, ReleaseFormCollectionDto>(body, EndpointsPaths.DrugLibrary_GetReleaseForms, cancellationToken);
    }
}