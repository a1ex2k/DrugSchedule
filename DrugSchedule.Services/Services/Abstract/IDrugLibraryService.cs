using DrugSchedule.Services.Models;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Services.Services.Abstractions;

public interface IDrugLibraryService
{
    public Task<ReleaseFormCollection> GetReleaseFormsAsync(MedicamentReleaseFormFilter filter, CancellationToken cancellationToken = default);

    public Task<MedicamentSimpleCollection> GetMedicamentsSimpleAsync(MedicamentFilter filter, CancellationToken cancellationToken = default);

    public Task<OneOf<MedicamentSimpleModel, NotFound>> GetMedicamentSimpleAsync(int id, CancellationToken cancellationToken = default);

    public Task<MedicamentExtendedCollection> GetMedicamentsExtendedAsync(MedicamentFilter filter, CancellationToken cancellationToken = default);

    public Task<OneOf<MedicamentExtendedModel, NotFound>> GetMedicamentExtendedAsync(int id, CancellationToken cancellationToken = default);

    public Task<ManufacturerCollection> GetManufacturersAsync(ManufacturerFilter filter, CancellationToken cancellationToken = default);

    public Task<OneOf<Manufacturer, NotFound>> GetManufacturerAsync(int id, CancellationToken cancellationToken = default);
}