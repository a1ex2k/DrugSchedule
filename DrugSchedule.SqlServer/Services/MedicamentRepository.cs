using DrugSchedule.SqlServer.Data;
using DrugSchedule.StorageContract.Abstractions;
using DrugSchedule.StorageContract.Data.MedicamentStorage;
using DrugSchedule.StorageContract.Data.MedicamentStorage.Filters;
using Microsoft.Extensions.Logging;

namespace DrugSchedule.SqlServer.Services;

public class MedicamentRepository : IMedicamentRepository
{
    private readonly DrugScheduleContext _dbContext;
    private readonly ILogger<MedicamentRepository> _logger;

    public MedicamentRepository(DrugScheduleContext dbContext, ILogger<MedicamentRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<List<Medicament>> GetMedicamentsAsync(MedicamentFilter filter, int skip, int count)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Manufacturer>> GetManufacturersAsync(ManufacturerFilter filter, int skip, int count)
    {
        throw new NotImplementedException();
    }

    public async Task<List<MedicamentReleaseForm>> GetMedicamentReleaseFormsAsync(MedicamentReleaseFormFilter filter, int skip, int count)
    {
        throw new NotImplementedException();
    }

    public async Task<Medicament> GetMedicamentsByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Manufacturer> GetManufacturerByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<MedicamentReleaseForm> GetMedicamentReleaseFormByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Medicament> UpdateMedicamentAsync(Medicament medicament)
    {
        throw new NotImplementedException();
    }

    public async Task<Manufacturer> UpdateManufacturerAsync(Manufacturer medicament)
    {
        throw new NotImplementedException();
    }

    public async Task<MedicamentReleaseForm> UpdateMedicamentReleaseForAsync(MedicamentReleaseForm releaseForm)
    {
        throw new NotImplementedException();
    }

    public async Task<Medicament> CreateMedicamentAsync(Medicament medicament)
    {
        throw new NotImplementedException();
    }

    public async Task<Manufacturer> CreateManufacturerAsync(Manufacturer medicament)
    {
        throw new NotImplementedException();
    }

    public async Task<MedicamentReleaseForm> CreateMedicamentReleaseForAsync(MedicamentReleaseForm releaseForm)
    {
        throw new NotImplementedException();
    }

    public async Task RemoveMedicamentsByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task RemoveManufacturerByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task RemoveMedicamentReleaseFormByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}