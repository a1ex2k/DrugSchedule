using DrugSchedule.Services.Utils;
using DrugSchedule.Services.Models;
using DrugSchedule.Services.Services.Abstractions;
using DrugSchedule.StorageContract.Abstractions;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Services.Services;

public class DrugLibraryService : IDrugLibraryService
{
    private readonly IReadonlyDrugRepository _repository;
    private readonly IFileService _fileService;
    private readonly IDownloadableFileConverter _downloadableFileConverter;
    
    public DrugLibraryService(IReadonlyDrugRepository repository, IDownloadableFileConverter downloadableFileConverter, IFileService fileService)
    {
        _repository = repository;
        _downloadableFileConverter = downloadableFileConverter;
        _fileService = fileService;
    }

    public async Task<ReleaseFormCollection> GetReleaseFormsAsync(MedicamentReleaseFormFilter filter, CancellationToken cancellationToken = default)
    {
        var releaseForms = await _repository.GetMedicamentReleaseFormsAsync(filter, cancellationToken);
        return new ReleaseFormCollection
        {
            ReleaseForms = releaseForms
        };
    }

    public async Task<OneOf<MedicamentReleaseForm, NotFound>> GetReleaseFormAsync(int id, CancellationToken cancellationToken = default)
    {
        var releaseForm = await _repository.GetMedicamentReleaseFormByIdAsync(id, cancellationToken);
        if (releaseForm == null)
        {
            return new NotFound($"Release form with ID={id} not found");
        }
        return releaseForm;
    }

    public async Task<MedicamentSimpleCollection> GetMedicamentsAsync(MedicamentFilter filter, CancellationToken cancellationToken = default)
    {
        var medicaments = await _repository.GetMedicamentsSimpleAsync(filter, cancellationToken);
        return new MedicamentSimpleCollection
        {
            Medicaments = medicaments.ConvertAll(ToModel)
        };
    }

    public async Task<OneOf<MedicamentSimpleModel, NotFound>> GetMedicamentAsync(int id, CancellationToken cancellationToken = default)
    {
        var medicament = await _repository.GetMedicamentSimpleByIdAsync(id, cancellationToken);
        if (medicament == null)
        {
            return new NotFound($"Medicament with ID={id} not found");
        }
        return ToModel(medicament);
    }

    public async Task<MedicamentExtendedCollection> GetMedicamentsExtendedAsync(MedicamentFilter filter, CancellationToken cancellationToken = default)
    {
        var medicaments = await _repository.GetMedicamentsExtendedAsync(filter, true, cancellationToken);
        var model = new MedicamentExtendedCollection
        {
            Medicaments = medicaments.ConvertAll(ToModel)
        };
        return model;
    }

    public async Task<OneOf<MedicamentExtendedModel, NotFound>> GetMedicamentExtendedAsync(int id, CancellationToken cancellationToken = default)
    {
        var medicament = await _repository.GetMedicamentExtendedByIdAsync(id, true, cancellationToken);
        if (medicament == null)
        {
            return new NotFound($"Medicament form with ID={id} not found");
        }

        var model = ToModel(medicament);
        return model;
    }


    public async Task<ManufacturerCollection> GetManufacturersAsync(ManufacturerFilter filter, CancellationToken cancellationToken = default)
    {
        var manufacturers = await _repository.GetManufacturersAsync(filter, cancellationToken);
        return new ManufacturerCollection
        {
            Manufacturers = manufacturers
        };
    }

    public async Task<OneOf<Manufacturer, NotFound>> GetManufacturerAsync(int id, CancellationToken cancellationToken = default)
    {
        var manufacturer = await _repository.GetManufacturerByIdAsync(id, cancellationToken);
        if (manufacturer == null)
        {
            return new NotFound($"Manufacturer with ID={id} not found");
        }
        return manufacturer;
    }

    public async Task<bool> DoesMedicamentExistAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.DoesMedicamentExistAsync(id, cancellationToken);
    }

    private MedicamentExtendedModel ToModel(MedicamentExtended medicament)
    {
        var model = new MedicamentExtendedModel
        {
            Id = medicament.Id,
            Name = medicament.Name,
            Composition = medicament.Composition,
            Description = medicament.Description,
            ReleaseForm = medicament.ReleaseForm,
            Manufacturer = medicament.Manufacturer,
            FileCollection = 
                new FileCollection
                {
                    Files = _downloadableFileConverter.ToFilesModels(medicament.Images!, FileCategory.MedicamentImage.IsPublic())
                }
        };
        return model;
    }

    private MedicamentSimpleModel ToModel(MedicamentSimple medicament)
    {
        var model = new MedicamentSimpleModel
        {
            Id = medicament.Id,
            Name = medicament.Name,
            ReleaseForm = medicament.ReleaseForm,
            ManufacturerName = medicament.ManufacturerName,
            ThumbnailUrl = _downloadableFileConverter.ToThumbLink(medicament.MainImage, FileCategory.MedicamentImage.IsPublic(), true) 
        };
        return model;
    }
}