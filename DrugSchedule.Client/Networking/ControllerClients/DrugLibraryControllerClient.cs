using DrugSchedule.Api.Shared.Dtos;

namespace DrugSchedule.Client.Networking;

public class DrugLibraryControllerClient : IDrugLibraryControllerClient
{
    private readonly IApiClient _client;

    public DrugLibraryControllerClient(IApiClient client)
    {
        _client = client;
    }

    public async Task<ApiCallResult<MedicamentSimpleDto>> GetMedicamentAsync(MedicamentIdDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync<MedicamentIdDto, MedicamentSimpleDto>(body, EndpointsPaths.DrugLibrary_GetMedicament, cancellationToken);
    }

    public async Task<ApiCallResult<MedicamentExtendedDto>> GetMedicamentExtendedAsync(MedicamentIdDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync<MedicamentIdDto, MedicamentExtendedDto>(body, EndpointsPaths.DrugLibrary_GetMedicamentExtended, cancellationToken);
    }

    public async Task<ApiCallResult<MedicamentSimpleCollectionDto>> GetMedicamentsAsync(MedicamentFilterDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync<MedicamentFilterDto, MedicamentSimpleCollectionDto>(body, EndpointsPaths.DrugLibrary_GetMedicaments, cancellationToken);
    }

    public async Task<ApiCallResult<MedicamentExtendedCollectionDto>> GetMedicamentsExtendedAsync(MedicamentFilterDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync<MedicamentFilterDto, MedicamentExtendedCollectionDto>(body, EndpointsPaths.DrugLibrary_GetMedicamentsExtended, cancellationToken);
    }

    public async Task<ApiCallResult<ManufacturerDto>> GetManufacturerAsync(ManufacturerIdDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync<ManufacturerIdDto, ManufacturerDto>(body, EndpointsPaths.DrugLibrary_GetManufacturer, cancellationToken);
    }

    public async Task<ApiCallResult<ManufacturerCollectionDto>> GetManufacturersAsync(ManufacturerFilterDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync<ManufacturerFilterDto, ManufacturerCollectionDto>(body, EndpointsPaths.DrugLibrary_GetManufacturers, cancellationToken);
    }

    public async Task<ApiCallResult<ReleaseFormCollectionDto>> GetReleaseFormsAsync(MedicamentReleaseFormFilterDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync<MedicamentReleaseFormFilterDto, ReleaseFormCollectionDto>(body, EndpointsPaths.DrugLibrary_GetReleaseForms, cancellationToken);
    }
}