using DrugSchedule.Storage.Services;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.BusinessLogic.Services;

public class DrugLibraryService : IDrugLibraryService
{
    private readonly DrugRepository _repository;
    private readonly FileInfoRepository _fileInfoRepository;

    public DrugLibraryService(DrugRepository repository, FileInfoRepository fileInfoRepository)
    {
        _repository = repository;
        _fileInfoRepository = fileInfoRepository;
    }

    public async Task<List<MedicamentReleaseForm>> GetMedicamentReleaseFormsAsync(MedicamentReleaseFormFilter filter, int skip, int count,
        CancellationToken cancellationToken = default)
    {
        var releaseForms = await _repository.GetMedicamentReleaseFormsAsync(filter, skip, count, cancellationToken);
        return releaseForms;
    }

    public async Task<MedicamentReleaseForm?> GetMedicamentReleaseFormByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var releaseForm = await _repository.GetMedicamentReleaseFormByIdAsync(id, cancellationToken);
        return releaseForm;
    }

    public async Task<List<Medicament>> GetMedicamentsAsync(MedicamentFilter filter, int skip, int count, CancellationToken cancellationToken = default)
    {
        var medicaments = await _repository.GetMedicamentsAsync(filter, skip, count, cancellationToken);
        var imagesInfo 
        return medicaments;
    }

    public async Task<Medicament?> GetMedicamentByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var medicament = await _repository.GetMedicamentByIdAsync(id, cancellationToken);
        return medicament;
    }

    public async Task<List<Manufacturer>> GetManufacturersAsync(ManufacturerFilter filter, int skip, int count,
        CancellationToken cancellationToken = default)
    {
        var manufacturers = await _repository.GetManufacturersAsync(filter, skip, count, cancellationToken);
        return manufacturers;
    }

    public async Task<Manufacturer?> GetManufacturerByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var manufacturer = await _repository.GetManufacturerByIdAsync(id, cancellationToken);
        return manufacturer;
    }
}


public interface IDrugLibraryService
{
    public Task<List<MedicamentReleaseForm>> GetMedicamentReleaseFormsAsync(MedicamentReleaseFormFilter filter, int skip, int count, CancellationToken cancellationToken = default);

    public Task<MedicamentReleaseForm?> GetMedicamentReleaseFormByIdAsync(int id, CancellationToken cancellationToken = default);

    public Task<List<Medicament>> GetMedicamentsAsync(MedicamentFilter filter, int skip, int count, CancellationToken cancellationToken = default);

    public Task<Medicament?> GetMedicamentByIdAsync(int id, CancellationToken cancellationToken = default);

    public Task<List<Manufacturer>> GetManufacturersAsync(ManufacturerFilter filter, int skip, int count, CancellationToken cancellationToken = default);

    public Task<Manufacturer?> GetManufacturerByIdAsync(int id, CancellationToken cancellationToken = default);
}