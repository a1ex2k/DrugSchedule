using DrugSchedule.StorageContract.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace DrugSchedule.StorageContract.Abstractions;

public interface IReadonlyDrugRepository
{
    public Task<List<MedicamentReleaseForm>> GetMedicamentReleaseFormsAsync(MedicamentReleaseFormFilter filter, CancellationToken cancellationToken = default);
    public Task<MedicamentReleaseForm?> GetMedicamentReleaseFormByIdAsync(int id, CancellationToken cancellationToken = default);


    public Task<List<MedicamentExtended>> GetMedicamentsExtendedAsync(MedicamentFilter filter, bool withImages, CancellationToken cancellationToken = default);
    public Task<MedicamentExtended?> GetMedicamentExtendedByIdAsync(int id, bool withImages, CancellationToken cancellationToken = default);
    public Task<List<MedicamentSimple>> GetMedicamentsSimpleAsync(MedicamentFilter filter, CancellationToken cancellationToken = default);
    public Task<MedicamentSimple?> GetMedicamentSimpleByIdAsync(int id, CancellationToken cancellationToken = default);
    public Task<bool> DoesMedicamentExistAsync(int id, CancellationToken cancellationToken = default);



    public Task<List<Manufacturer>> GetManufacturersAsync(ManufacturerFilter filter, CancellationToken cancellationToken = default);
    public Task<Manufacturer?> GetManufacturerByIdAsync(int id, CancellationToken cancellationToken = default);
}