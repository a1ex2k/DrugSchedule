using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.BusinessLogic.Services.Abstractions;

public interface IDrugLibraryService
{
    public Task<List<MedicamentReleaseForm>> GetMedicamentReleaseFormsAsync(MedicamentReleaseFormFilter filter, int skip, int count, CancellationToken cancellationToken = default);

    public Task<MedicamentReleaseForm?> GetMedicamentReleaseFormByIdAsync(int id, CancellationToken cancellationToken = default);

    public Task<List<Medicament>> GetMedicamentsAsync(MedicamentFilter filter, int skip, int count, CancellationToken cancellationToken = default);

    public Task<Medicament?> GetMedicamentByIdAsync(int id, CancellationToken cancellationToken = default);

    public Task<List<Manufacturer>> GetManufacturersAsync(ManufacturerFilter filter, int skip, int count, CancellationToken cancellationToken = default);

    public Task<Manufacturer?> GetManufacturerByIdAsync(int id, CancellationToken cancellationToken = default);
}