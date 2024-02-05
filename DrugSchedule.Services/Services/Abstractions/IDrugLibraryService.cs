using DrugSchedule.BusinessLogic.Models;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.BusinessLogic.Services.Abstractions;

public interface IDrugLibraryService
{
    public Task<ReleaseFormCollection> GetReleaseFormsAsync(MedicamentReleaseFormFilter filter, CancellationToken cancellationToken = default);

    public Task<OneOf<MedicamentReleaseForm, NotFound>> GetReleaseFormAsync(int id, CancellationToken cancellationToken = default);

    public Task<MedicamentSimpleCollection> GetMedicamentsAsync(MedicamentFilter filter, CancellationToken cancellationToken = default);

    public Task<OneOf<MedicamentSimpleModel, NotFound>> GetMedicamentAsync(int id, CancellationToken cancellationToken = default);

    public Task<MedicamentExtendedCollection> GetMedicamentsExtendedAsync(MedicamentFilter filter, CancellationToken cancellationToken = default);

    public Task<OneOf<MedicamentExtendedModel, NotFound>> GetMedicamentExtendedAsync(int id, CancellationToken cancellationToken = default);

    public Task<ManufacturerCollection> GetManufacturersAsync(ManufacturerFilter filter, CancellationToken cancellationToken = default);

    public Task<OneOf<Manufacturer, NotFound>> GetManufacturerAsync(int id, CancellationToken cancellationToken = default);
    
    public Task<bool> DoesMedicamentExistAsync(int id, CancellationToken cancellationToken = default);
}