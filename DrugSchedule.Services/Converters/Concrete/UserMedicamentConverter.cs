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

    public UserMedicamentUpdate ToUpdateResultModel(UserMedicamentPlain medicamentPlain)
    {
        return new UserMedicamentUpdate
        {
            Id = medicamentPlain.Id,
            BasicMedicamentId = medicamentPlain.BasicMedicamentId,
            Name = medicamentPlain.Name,
            ReleaseForm = medicamentPlain.ReleaseForm,
            Description = medicamentPlain.Description,
            Composition = medicamentPlain.Composition,
            ManufacturerName = medicamentPlain.ManufacturerName,
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