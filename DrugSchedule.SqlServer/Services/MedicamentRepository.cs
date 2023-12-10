using DrugSchedule.SqlServer.Data;
using DrugSchedule.StorageContract.Abstractions;
using DrugSchedule.StorageContract.Data;
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


    public Task<List<Medicament>> GetMedicamentsAsync(MedicamentFilter filter, int skip, int count)
    {
        throw new NotImplementedException();
    }


    public Task<List<Manufacturer>> GetManufacturersAsync(ManufacturerFilter filter, int skip, int count)
    {
        throw new NotImplementedException();
    }


    public Task<List<MedicamentReleaseForm>> GetMedicamentReleaseFormsAsync(MedicamentReleaseFormFilter filter, int skip, int count)
    {
        throw new NotImplementedException();
    }


    public Task<Medicament> GetMedicamentsByIdAsync(int id)
    {
        throw new NotImplementedException();
    }


    public Task<Manufacturer> GetManufacturerByIdAsync(int id)
    {
        throw new NotImplementedException();
    }


    public Task<MedicamentReleaseForm> GetMedicamentReleaseFormByIdAsync(int id)
    {
        throw new NotImplementedException();
    }


    public Task<Medicament> UpdateMedicamentAsync(Medicament medicament)
    {
        throw new NotImplementedException();
    }


    public Task<Manufacturer> UpdateManufacturerAsync(Manufacturer medicament)
    {
        throw new NotImplementedException();
    }


    public Task<MedicamentReleaseForm> UpdateMedicamentReleaseForAsync(MedicamentReleaseForm releaseForm)
    {
        throw new NotImplementedException();
    }


    public Task<Medicament> CreateMedicamentAsync(Medicament medicament)
    {
        throw new NotImplementedException();
    }


    public Task<Manufacturer> CreateManufacturerAsync(Manufacturer medicament)
    {
        throw new NotImplementedException();
    }


    public Task<MedicamentReleaseForm> CreateMedicamentReleaseForAsync(MedicamentReleaseForm releaseForm)
    {
        throw new NotImplementedException();
    }


    public Task RemoveMedicamentsByIdAsync(int id)
    {
        throw new NotImplementedException();
    }


    public Task RemoveManufacturerByIdAsync(int id)
    {
        throw new NotImplementedException();
    }


    public Task RemoveMedicamentReleaseFormByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}