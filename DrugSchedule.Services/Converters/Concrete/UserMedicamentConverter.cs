using DrugSchedule.Services.Models;
using DrugSchedule.Services.Utils;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Services.Converters;

public class UserMedicamentConverter : IUserMedicamentConverter
{
    private readonly IDownloadableFileConverter _downloadableFileConverter;
    private readonly IGlobalMedicamentConverter _globalMedicamentConverter;

    public UserMedicamentConverter(IDownloadableFileConverter downloadableFileConverter, IGlobalMedicamentConverter globalMedicamentConverter)
    {
        _downloadableFileConverter = downloadableFileConverter;
        _globalMedicamentConverter = globalMedicamentConverter;
    }

    public UserMedicamentUpdate ToUpdateResultModel(UserMedicament medicament)
    {
        return new UserMedicamentUpdate
        {
            Id = medicament.Id,
            BasicMedicamentId = medicament.BasicMedicamentId,
            Name = medicament.Name,
            ReleaseForm = medicament.ReleaseForm,
            Description = medicament.Description,
            Composition = medicament.Composition,
            ManufacturerName = medicament.ManufacturerName,
        };
    }

    public UserMedicamentExtendedModel ToUserMedicamentExtended(UserMedicamentExtended userMedicament, MedicamentExtended? globalMedicament)
    {
        var model = new UserMedicamentExtendedModel
        {
            Id = userMedicament.Id,
            Name = userMedicament.Name,
            Composition = userMedicament.Composition,
            Description = userMedicament.Description,
            ReleaseForm = userMedicament.ReleaseForm,
            ManufacturerName = userMedicament.ManufacturerName,
            BasicMedicament = globalMedicament == null ? null : _globalMedicamentConverter.ToMedicamentExtended(globalMedicament),
            Images =
                new FileCollection
                {
                    Files = _downloadableFileConverter.ToFilesModels(userMedicament.Images!, FileCategory.UserMedicamentImage.IsPublic())
                }
        };
        return model;
    }

    public UserMedicamentSimpleModel ToUserMedicamentSimple(UserMedicamentSimple medicament)
    {
        var model = new UserMedicamentSimpleModel
        {
            Id = medicament.Id,
            Name = medicament.Name,
            ReleaseForm = medicament.ReleaseForm,
            ManufacturerName = medicament.ManufacturerName,
            ThumbnailUrl = _downloadableFileConverter.ToThumbLink(medicament.MainImage, FileCategory.UserMedicamentImage.IsPublic())
        };
        return model;
    }

    public UserMedicamentSimpleCollection ToUserMedicamentSimpleCollection(List<UserMedicamentSimple> userMedicaments)
    {
        return new UserMedicamentSimpleCollection
        {
            Medicaments = userMedicaments.ConvertAll(ToUserMedicamentSimple)
        };
    }
}