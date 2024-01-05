﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DrugSchedule.StorageContract.Data;
using DrugSchedule.StorageContract.Data.Common;

namespace DrugSchedule.StorageContract.Abstractions;

public interface IMedicamentRepository
{
    public Task<List<Medicament>> GetMedicamentsAsync(MedicamentFilter filter, int skip, int count);
    public Task<List<Manufacturer>> GetManufacturersAsync(ManufacturerFilter filter, int skip, int count);
    public Task<List<MedicamentReleaseForm>> GetMedicamentReleaseFormsAsync(MedicamentReleaseFormFilter filter, int skip, int count);

    public Task<Medicament?> GetMedicamentByIdAsync(int id);
    public Task<Manufacturer?> GetManufacturerByIdAsync(int id);
    public Task<MedicamentReleaseForm?> GetMedicamentReleaseFormByIdAsync(int id);

    public Task<Medicament?> UpdateMedicamentAsync(Medicament medicament);
    public Task<Manufacturer?> UpdateManufacturerAsync(Manufacturer medicament);
    public Task<MedicamentReleaseForm?> UpdateMedicamentReleaseForAsync(MedicamentReleaseForm releaseForm);

    public Task<Medicament?> CreateMedicamentAsync(Medicament medicament);
    public Task<Manufacturer?> CreateManufacturerAsync(Manufacturer medicament);
    public Task<MedicamentReleaseForm?> CreateMedicamentReleaseForAsync(MedicamentReleaseForm releaseForm);

    public Task<RemoveOperationResult> RemoveMedicamentByIdAsync(int id);
    public Task<RemoveOperationResult> RemoveManufacturerByIdAsync(int id);
    public Task<RemoveOperationResult> RemoveMedicamentReleaseFormByIdAsync(int id);
}