using System;
using System.Threading;
using System.Threading.Tasks;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.StorageContract.Abstractions;

public interface IDrugRepository : IReadonlyDrugRepository
{
    public Task<MedicamentReleaseForm?> UpdateMedicamentReleaseFormAsync(MedicamentReleaseForm releaseForm, MedicamentReleaseFormUpdateFlags updateFlags, CancellationToken cancellationToken = default);

    public Task<MedicamentReleaseForm?> CreateMedicamentReleaseForAsync(MedicamentReleaseForm releaseForm, CancellationToken cancellationToken = default);

    public Task<RemoveOperationResult> RemoveMedicamentReleaseFormByIdAsync(int id, CancellationToken cancellationToken = default);

    public Task<MedicamentExtended?> UpdateMedicamentAsync(MedicamentExtended medicament, MedicamentUpdateFlags updateFlags, CancellationToken cancellationToken = default);

    public Task<MedicamentExtended?> CreateMedicamentAsync(MedicamentExtended medicament, CancellationToken cancellationToken = default);

    public Task<bool> RemoveMedicamentImageGuidAsync(int medicamentId, Guid imageGuid, CancellationToken cancellationToken = default);

    public Task<bool> AddMedicamentImageGuidAsync(int medicamentId, Guid imageGuid,
        CancellationToken cancellationToken = default);

    public Task<Manufacturer?> UpdateManufacturerAsync(Manufacturer manufacturer, ManufacturerUpdateFlags updateFlags, CancellationToken cancellationToken = default);

    public Task<Manufacturer?> CreateManufacturerAsync(Manufacturer manufacturer, CancellationToken cancellationToken = default);

    public Task<RemoveOperationResult> RemoveManufacturerByIdAsync(int id, CancellationToken cancellationToken = default);
}