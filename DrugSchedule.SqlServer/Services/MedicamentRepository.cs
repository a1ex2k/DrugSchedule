using DrugSchedule.SqlServer.Data;
using DrugSchedule.SqlServer.Extensions;
using DrugSchedule.StorageContract.Abstractions;
using DrugSchedule.StorageContract.Data;
using DrugSchedule.StorageContract.Data.Common;
using Microsoft.EntityFrameworkCore;
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
        var medicaments = await _dbContext.Medicaments
            .AsNoTracking()
            .Include(m => m.ReleaseForm)
            .Include(m => m.Manufacturer)
            .Include(m => m.Images)
            .WithFilter(m => m.Id, filter.IdFilter)
            .WithFilter(m => m.Name, filter.NameFilter)
            .WithFilter(m => m.ManufacturerId, filter.ManufacturerFilter?.IdFilter)
            .WithFilter(m => m.Manufacturer!.Name, filter.ManufacturerFilter?.NameFilter)
            .WithFilter(m => m.ReleaseFormId, filter.MedicamentReleaseFormFilter?.IdFilter)
            .WithFilter(m => m.ReleaseForm!.Name, filter.MedicamentReleaseFormFilter?.NameFilter)
            .OrderBy(m => m.Name)
            .WithPaging(skip, count)
            .Select(m => m.ToContractMedicament())
            .ToListAsync();

        return medicaments;
    }


    public async Task<List<Manufacturer>> GetManufacturersAsync(ManufacturerFilter filter, int skip, int count)
    {
        var manufacturers = await _dbContext.Manufacturers
            .AsNoTracking()
            .WithFilter(m => m.Id, filter.IdFilter)
            .WithFilter(m => m.Name, filter.NameFilter)
            .OrderBy(m => m.Name)
            .WithPaging(skip, count)
            .Select(m => m.ToContractManufacturer())
            .ToListAsync();

        return manufacturers;
    }


    public async Task<List<MedicamentReleaseForm>> GetMedicamentReleaseFormsAsync(MedicamentReleaseFormFilter filter, int skip, int count)
    {
        var releaseForms = await _dbContext.MedicamentReleaseForms
            .AsNoTracking()
            .WithFilter(m => m.Id, filter.IdFilter)
            .WithFilter(m => m.Name, filter.NameFilter)
            .OrderBy(m => m.Name)
            .WithPaging(skip, count)
            .Select(m => m.ToContractReleaseForm())
            .ToListAsync();

        return releaseForms;
    }


    public async Task<Medicament?> GetMedicamentByIdAsync(int id)
    {
        var medicament = await _dbContext.Medicaments
            .AsNoTracking()
            .Include(m => m.ReleaseForm)
            .Include(m => m.Manufacturer)
            .Include(m => m.Images)
            .FirstOrDefaultAsync(m => m.Id == id);

        return medicament?.ToContractMedicament();
    }


    public async Task<Manufacturer?> GetManufacturerByIdAsync(int id)
    {
        var manufacturer = await _dbContext.Manufacturers
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);

        return manufacturer?.ToContractManufacturer();
    }


    public async Task<MedicamentReleaseForm?> GetMedicamentReleaseFormByIdAsync(int id)
    {
        var releaseForm = await _dbContext.MedicamentReleaseForms
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == id);

        return releaseForm?.ToContractReleaseForm();
    }


    public Task<Medicament?> UpdateMedicamentAsync(Medicament medicament)
    {
        throw new NotImplementedException();
    }


    public Task<Manufacturer?> UpdateManufacturerAsync(Manufacturer medicament)
    {
        throw new NotImplementedException();
    }


    public Task<MedicamentReleaseForm?> UpdateMedicamentReleaseForAsync(MedicamentReleaseForm releaseForm)
    {
        throw new NotImplementedException();
    }


    public Task<Medicament?> CreateMedicamentAsync(Medicament medicament)
    {
        throw new NotImplementedException();
    }


    public Task<Manufacturer?> CreateManufacturerAsync(Manufacturer medicament)
    {
        throw new NotImplementedException();
    }


    public Task<MedicamentReleaseForm?> CreateMedicamentReleaseForAsync(MedicamentReleaseForm releaseForm)
    {
        throw new NotImplementedException();
    }


    public async Task<RemoveOperationResult> RemoveMedicamentByIdAsync(int id)
    {
        if (!await _dbContext.Manufacturers)
    }


    public async Task<RemoveOperationResult> RemoveManufacturerByIdAsync(int id)
    {
        var manufacturer = await _dbContext.Manufacturers
            .FirstOrDefaultAsync(m => m.Id == id);

        if (manufacturer is null)
        {
            return RemoveOperationResult.NotExists;
        }
        
        await _dbContext.Manufacturers.

        try
        {

        }


    }


    public Task<RemoveOperationResult> RemoveMedicamentReleaseFormByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}