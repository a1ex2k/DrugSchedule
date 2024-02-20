using DrugSchedule.Services.Models;
using DrugSchedule.Services.Services.Abstractions;
using DrugSchedule.StorageContract.Abstractions;
using DrugSchedule.StorageContract.Data;
using DrugSchedule.Services.Converters;
using DrugSchedule.Services.Utils;

namespace DrugSchedule.Services.Services;

public class DrugLibraryService : IDrugLibraryService
{
    private readonly IReadonlyDrugRepository _repository;
    private readonly IFileService _fileService;
    private readonly IGlobalMedicamentConverter _converter;
    
    public DrugLibraryService(IReadonlyDrugRepository repository, IFileService fileService, IGlobalMedicamentConverter converter)
    {
        _repository = repository;
        _converter = converter;
        _fileService = fileService;
    }

    public async Task<ReleaseFormCollection> GetReleaseFormsAsync(MedicamentReleaseFormFilter filter, CancellationToken cancellationToken = default)
    {
        filter.LimitSimple();
        var releaseForms = await _repository.GetMedicamentReleaseFormsAsync(filter, cancellationToken);
        return _converter.ToReleaseFormCollection(releaseForms);
    }

    public async Task<OneOf<MedicamentReleaseForm, NotFound>> GetReleaseFormAsync(int id, CancellationToken cancellationToken = default)
    {
        var releaseForm = await _repository.GetMedicamentReleaseFormByIdAsync(id, cancellationToken);
        if (releaseForm == null)
        {
            return new NotFound(ErrorMessages.ReleaseFormNotFound);
        }

        return releaseForm;
    }

    public async Task<MedicamentSimpleCollection> GetMedicamentsSimpleAsync(MedicamentFilter filter, CancellationToken cancellationToken = default)
    {
        filter.LimitSimple();
        var medicaments = await _repository.GetMedicamentsSimpleAsync(filter, cancellationToken);
        return _converter.ToMedicamentSimpleCollection(medicaments);
    }

    public async Task<OneOf<MedicamentSimpleModel, NotFound>> GetMedicamentSimpleAsync(int id, CancellationToken cancellationToken = default)
    {
        var medicament = await _repository.GetMedicamentSimpleByIdAsync(id, cancellationToken);
        if (medicament == null)
        {
            return new NotFound(ErrorMessages.MedicamentNotFound);
        }

        return _converter.ToMedicamentSimple(medicament);
    }

    public async Task<MedicamentExtendedCollection> GetMedicamentsExtendedAsync(MedicamentFilter filter, CancellationToken cancellationToken = default)
    {
        filter.LimitExtended();
        var medicaments = await _repository.GetMedicamentsExtendedAsync(filter, true, cancellationToken);
        return _converter.ToMedicamentExtendedCollection(medicaments);
    }

    public async Task<OneOf<MedicamentExtendedModel, NotFound>> GetMedicamentExtendedAsync(int id, CancellationToken cancellationToken = default)
    {
        var medicament = await _repository.GetMedicamentExtendedByIdAsync(id, true, cancellationToken);
        if (medicament == null)
        {
            return new NotFound(ErrorMessages.ReleaseFormNotFound);
        }

        return _converter.ToMedicamentExtended(medicament);
    }
    
    public async Task<ManufacturerCollection> GetManufacturersAsync(ManufacturerFilter filter, CancellationToken cancellationToken = default)
    {
        filter.LimitSimple();
        var manufacturers = await _repository.GetManufacturersAsync(filter, cancellationToken);
        return _converter.ToManufacturerCollection(manufacturers);
    }

    public async Task<OneOf<Manufacturer, NotFound>> GetManufacturerAsync(int id, CancellationToken cancellationToken = default)
    {
        var manufacturer = await _repository.GetManufacturerByIdAsync(id, cancellationToken);
        if (manufacturer == null)
        {
            return new NotFound(ErrorMessages.ManufacturerNotFound);
        }
        return manufacturer;
    }
}