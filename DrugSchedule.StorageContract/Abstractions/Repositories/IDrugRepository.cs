using DrugSchedule.StorageContract.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace DrugSchedule.StorageContract.Abstractions;

public interface IDrugRepository
{
    public Task<List<MedicamentReleaseForm>> GetMedicamentReleaseFormsAsync(MedicamentReleaseFormFilter filter, int skip, int count, CancellationToken cancellationToken = default);

    public Task<MedicamentReleaseForm?> GetMedicamentReleaseFormByIdAsync(int id, CancellationToken cancellationToken = default);

    public Task<MedicamentReleaseForm?> UpdateMedicamentReleaseFormAsync(MedicamentReleaseForm releaseForm, MedicamentReleaseFormUpdateFlags updateFlags, CancellationToken cancellationToken = default);

    public Task<MedicamentReleaseForm?> CreateMedicamentReleaseForAsync(MedicamentReleaseForm releaseForm, CancellationToken cancellationToken = default);

    public Task<RemoveOperationResult> RemoveMedicamentReleaseFormByIdAsync(int id, CancellationToken cancellationToken = default);

    public Task<List<Medicament>> GetMedicamentsAsync(MedicamentFilter filter, int skip, int count, CancellationToken cancellationToken = default);

    public Task<Medicament?> GetMedicamentByIdAsync(int id, CancellationToken cancellationToken = default);

    public Task<Medicament?> UpdateMedicamentAsync(Medicament medicament, MedicamentUpdateFlags updateFlags, CancellationToken cancellationToken = default);

    public Task<Medicament?> CreateMedicamentAsync(Medicament medicament, CancellationToken cancellationToken = default);

    public Task<RemoveOperationResult> RemoveMedicamentByIdAsync(int id, CancellationToken cancellationToken = default);

    public Task<bool> AddMedicamentImageGuidAsync(int medicamentId, Guid imageGuid, CancellationToken cancellationToken = default);

    public Task<bool> RemoveMedicamentImageGuidAsync(int medicamentId, Guid imageGuid, CancellationToken cancellationToken = default);

    public Task<List<Manufacturer>> GetManufacturersAsync(ManufacturerFilter filter, int skip, int count, CancellationToken cancellationToken = default);

    public Task<Manufacturer?> GetManufacturerByIdAsync(int id, CancellationToken cancellationToken = default);

    public Task<Manufacturer?> UpdateManufacturerAsync(Manufacturer manufacturer, ManufacturerUpdateFlags updateFlags, CancellationToken cancellationToken = default);

    public Task<Manufacturer?> CreateManufacturerAsync(Manufacturer manufacturer, CancellationToken cancellationToken = default);

    public Task<RemoveOperationResult> RemoveManufacturerByIdAsync(int id, CancellationToken cancellationToken = default);
}