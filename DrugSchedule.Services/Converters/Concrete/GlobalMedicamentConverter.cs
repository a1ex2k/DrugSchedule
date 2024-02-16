using DrugSchedule.Services.Models;
using DrugSchedule.Services.Utils;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Services.Converters;

public class GlobalMedicamentConverter : IGlobalMedicamentConverter
{
    private readonly IDownloadableFileConverter _downloadableFileConverter;

    public GlobalMedicamentConverter(IDownloadableFileConverter downloadableFileConverter)
    {
        _downloadableFileConverter = downloadableFileConverter;
    }

    public MedicamentExtendedModel ToMedicamentExtended(MedicamentExtended medicament)
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
                    Files = _downloadableFileConverter.ToFilesModels(medicament.Images!,
                        FileCategory.MedicamentImage.IsPublic())
                }
        };
        return model;
    }

    public MedicamentSimpleModel ToMedicamentSimple(MedicamentSimple medicament)
    {
        var model = new MedicamentSimpleModel
        {
            Id = medicament.Id,
            Name = medicament.Name,
            ReleaseForm = medicament.ReleaseForm,
            ManufacturerName = medicament.ManufacturerName,
            ThumbnailUrl =
                _downloadableFileConverter.ToThumbLink(medicament.MainImage, FileCategory.MedicamentImage.IsPublic(),
                    true)
        };
        return model;
    }

    public MedicamentSimpleCollection ToMedicamentSimpleCollection(List<MedicamentSimple> medicaments)
    {
        return new MedicamentSimpleCollection
        {
            Medicaments = medicaments.ConvertAll(ToMedicamentSimple)
        };
    }

    public MedicamentExtendedCollection ToMedicamentExtendedCollection(List<MedicamentExtended> medicaments)
    {
        return new MedicamentExtendedCollection
        {
            Medicaments = medicaments.ConvertAll(ToMedicamentExtended)
        };
    }

    public ManufacturerCollection ToManufacturerCollection(List<Manufacturer> manufacturers)
    {
        return new ManufacturerCollection
        {
            Manufacturers = manufacturers
        };
    }

    public ReleaseFormCollection ToReleaseFormCollection(List<MedicamentReleaseForm> forms)
    {
        return new ReleaseFormCollection
        {
            ReleaseForms = forms
        };
    }
}