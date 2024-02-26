using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Networking;

namespace DrugSchedule.Client.Networking;

public interface IDrugLibraryControllerClient
{
    Task<ApiCallResult<MedicamentSimpleDto>> GetMedicamentAsync(MedicamentIdDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<MedicamentExtendedDto>> GetMedicamentExtendedAsync(MedicamentIdDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<MedicamentSimpleCollectionDto>> GetMedicamentsAsync(MedicamentFilterDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<MedicamentExtendedCollectionDto>> GetMedicamentsExtendedAsync(MedicamentFilterDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<ManufacturerDto>> GetManufacturerAsync(ManufacturerIdDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<ManufacturerCollectionDto>> GetManufacturersAsync(ManufacturerFilterDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<ReleaseFormCollectionDto>> GetReleaseFormsAsync(MedicamentReleaseFormFilterDto body, CancellationToken cancellationToken = default);
}