using DrugSchedule.StorageContract.Abstractions;
using DrugSchedule.StorageContract.Data;
using DrugSchedule.Services.Models;
using DrugSchedule.Services.Services.Abstractions;
using DrugSchedule.Services.Utils;
using OneOf.Types;

namespace DrugSchedule.Services.Services;

public class UserDrugLibrary : IUserDrugLibrary
{
    private readonly ICurrentUserIdentifier _currentUserIdentifier;
    private readonly IUserDrugRepository _userDrugRepository;
    private readonly IDrugLibraryService _drugLibrary;
    private readonly IFileService _fileService;
    private readonly IDownloadableFileConverter _downloadableFileConverter;

    public UserDrugLibrary(IFileService fileService, ICurrentUserIdentifier currentUserIdentifier, IUserDrugRepository userDrugRepository, IDownloadableFileConverter downloadableFileConverter, IDrugLibraryService drugLibrary)
    {
        _fileService = fileService;
        _currentUserIdentifier = currentUserIdentifier;
        _userDrugRepository = userDrugRepository;
        _downloadableFileConverter = downloadableFileConverter;
        _drugLibrary = drugLibrary;
    }


    public async Task<OneOf<UserMedicamentExtendedModel, NotFound>> GetMedicamentExtendedAsync(long id,
        CancellationToken cancellationToken = default)
    {
        var medicament = await _userDrugRepository.GetMedicamentExtendedAsync(_currentUserIdentifier.UserProfileId, id,
            true, true, cancellationToken);

        if (medicament == null)
        {
            return new NotFound($"Current user doesn't have custom medicament with ID={id}");
        }

        if (medicament.BasicMedicamentId == null)
        {
            return ToModel(medicament, null);
        }

        var result =
            await _drugLibrary.GetMedicamentExtendedAsync(medicament.BasicMedicamentId.Value, cancellationToken);

        var globalMedicament = result.IsT0 ? result.AsT0 : null;
        return ToModel(medicament, globalMedicament);
    }

    public async Task<UserMedicamentExtendedCollection> GetMedicamentsExtendedAsync(UserMedicamentFilter filter, CancellationToken cancellationToken = default)
    {
        var medicaments = await _userDrugRepository.GetMedicamentsExtendedAsync(_currentUserIdentifier.UserProfileId, filter,
            true, true, cancellationToken);

        var basicIds = medicaments
            .Where(m => m.BasicMedicamentId != null)
            .Select(m => m.BasicMedicamentId!.Value)
            .ToList();

        var globalMedicaments = await _drugLibrary.GetMedicamentsExtendedAsync(
            new MedicamentFilter { IdFilter = basicIds }, cancellationToken);

        return new UserMedicamentExtendedCollection
        {
            Medicaments = medicaments.ConvertAll(m =>
            {
                var gm = m.BasicMedicamentId == null
                    ? null
                    : globalMedicaments.Medicaments.Find(x => x.Id == m.BasicMedicamentId.Value);
                return ToModel(m, gm);
            })
        };
    }

    public async Task<OneOf<UserMedicamentSimpleModel, NotFound>> GetMedicamentSimpleAsync(long id, CancellationToken cancellationToken = default)
    {
        var medicament = await _userDrugRepository.GetMedicamentSimpleAsync(_currentUserIdentifier.UserProfileId, id, cancellationToken);
        if (medicament == null)
        {
            return new NotFound($"Current user doesn't have custom medicament with ID={id}");
        }

        return ToModel(medicament);
    }

    public async Task<UserMedicamentSimpleCollection> GetMedicamentsSimpleAsync(UserMedicamentFilter filter, CancellationToken cancellationToken = default)
    {
        var medicaments = await _userDrugRepository.GetMedicamentsSimpleAsync(_currentUserIdentifier.UserProfileId, filter, cancellationToken);
        return new UserMedicamentSimpleCollection
        {
            Medicaments = medicaments.ConvertAll(ToModel)
        };
    }

    public async Task<OneOf<UserMedicamentUpdate, InvalidInput>> CreateMedicamentAsync(NewUserMedicament model, CancellationToken cancellationToken = default)
    {
        var invalidInput = new InvalidInput();
        if (model.BasicMedicamentId != null)
        {
            var exists = await _drugLibrary.DoesMedicamentExistAsync(model.BasicMedicamentId.Value, cancellationToken);
            if (!exists)
            {
                invalidInput.Add($"Base medicament with ID={model.BasicMedicamentId} not found");
            }
        }
        if (string.IsNullOrWhiteSpace(model.Name))
        {
            invalidInput.Add("Name must be non white space. ");
        }
        if (string.IsNullOrWhiteSpace(model.ReleaseForm))
        {
            invalidInput.Add("ReleaseForm must be non white space. ");
        }

        if (invalidInput.HasMessages) return invalidInput;

        var medicament = new UserMedicament
        {
            UserProfileId = _currentUserIdentifier.UserProfileId,
            BasicMedicamentId = model.BasicMedicamentId,
            Name = model.Name.Trim(),
            ReleaseForm = model.ReleaseForm.Trim(),
            Description = string.IsNullOrWhiteSpace(model.Description) ? null : model.Description.Trim(),
            Composition = string.IsNullOrWhiteSpace(model.Composition) ? null : model.Composition.Trim(),
            ManufacturerName = string.IsNullOrWhiteSpace(model.ManufacturerName) ? null : model.ManufacturerName.Trim(),
        };

        var savedMedicament = await _userDrugRepository.CreateMedicamentAsync(medicament, cancellationToken);
        return CreateUpdateResultModel(savedMedicament!);
    }

    public async Task<OneOf<UserMedicamentUpdate, NotFound, InvalidInput>> UpdateMedicamentAsync(UserMedicamentUpdate model, CancellationToken cancellationToken = default)
    {
        var medicament =
            await _userDrugRepository.GetMedicamentAsync(_currentUserIdentifier.UserProfileId, model.Id,
                cancellationToken);
        if (medicament == null)
        {
            return new NotFound($"Current user doesn't have custom medicament with ID={model.Id}");
        }

        var invalidInput = new InvalidInput();
        if (model.BasicMedicamentId != null && model.BasicMedicamentId != 0)
        {
            var exists = await _drugLibrary.DoesMedicamentExistAsync(model.BasicMedicamentId.Value, cancellationToken);
            if (!exists)
            {
                invalidInput.Add($"Base medicament with ID={model.BasicMedicamentId} not found. To remove, set BasicMedicamentId=0");
            }
        }
        if (model.Name != null && string.IsNullOrWhiteSpace(model.Name))
        {
            invalidInput.Add("Name must be non white space. Set null to keep current");
        }
        if (model.ReleaseForm != null && string.IsNullOrWhiteSpace(model.ReleaseForm))
        {
            invalidInput.Add("ReleaseForm must be non white space. Set null to keep current");
        }

        if (invalidInput.HasMessages) return invalidInput;

        var updateFlags = new UserMedicamentUpdateFlags
        {
            BasedOnMedicament = model.BasicMedicamentId != null,
            Name = model.Name != null,
            Description = model.Description != null,
            Composition = model.Composition != null,
            ReleaseForm = model.ReleaseForm != null,
            ManufacturerName = model.ManufacturerName != null,
        };

        medicament.BasicMedicamentId = model.BasicMedicamentId;
        medicament.Name = model.Name!.Trim();
        medicament.ReleaseForm = model.ReleaseForm!.Trim();
        medicament.Description = model.Description?.Trim();
        medicament.Composition = model.Composition?.Trim();
        medicament.ManufacturerName = model.ManufacturerName?.Trim();

        var savedMedicament = await _userDrugRepository.UpdateMedicamentAsync(medicament, updateFlags, cancellationToken);
        return CreateUpdateResultModel(savedMedicament!);
    }


    public async Task<OneOf<DownloadableFile, NotFound, InvalidInput>> AddImageAsync(long medicamentId, InputFile inputFile, CancellationToken cancellationToken = default)
    {
        var medicament =
            await _userDrugRepository.GetMedicamentAsync(_currentUserIdentifier.UserProfileId, medicamentId, cancellationToken);
        if (medicament == null)
        {
            return new NotFound($"Current user doesn't have custom medicament with ID={medicamentId}");
        }

        var addResult = await _fileService.CreateAsync(
            inputFile,
            FileCategory.UserMedicamentImage.GetAwaitableParams(),
            FileCategory.UserMedicamentImage,
            cancellationToken);

        if (addResult.IsT1)
        {
            return addResult.AsT1;
        }

        return _downloadableFileConverter.ToFileModel(addResult.AsT0, true)!;
    }

    public async Task<OneOf<True, NotFound>> RemoveImageAsync(long medicamentId, Guid fileGuid, CancellationToken cancellationToken = default)
    {
        var medicament =
            await _userDrugRepository.GetMedicamentAsync(_currentUserIdentifier.UserProfileId, medicamentId, cancellationToken);
        if (medicament == null)
        {
            return new NotFound($"Current user doesn't have custom medicament with ID={medicamentId}");
        }

        if (medicament.ImageGuids == null || !medicament.ImageGuids.Contains(fileGuid))
        {
            return new NotFound($"User medicament doesn't contain any image with provided Guid");

        }

        var updateFlags = new UserMedicamentUpdateFlags
        {
            Images = true
        };

        medicament.ImageGuids.Remove(fileGuid);
        var savedMedicament = await _userDrugRepository.UpdateMedicamentAsync(medicament, updateFlags, cancellationToken);
        if (savedMedicament != null)
        {
            await _fileService.RemoveFileAsync(fileGuid, cancellationToken);
        }
        return new True();
    }


    private UserMedicamentUpdate CreateUpdateResultModel(UserMedicament medicament)
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

    private UserMedicamentExtendedModel ToModel(UserMedicamentExtended userMedicament, MedicamentExtendedModel? globalMedicament)
    {
        var model = new UserMedicamentExtendedModel
        {
            Id = userMedicament.Id,
            Name = userMedicament.Name,
            Composition = userMedicament.Composition,
            Description = userMedicament.Description,
            ReleaseForm = userMedicament.ReleaseForm,
            ManufacturerName = userMedicament.ManufacturerName,
            BasicMedicament = globalMedicament,
            Images =
                new FileCollection
                {
                    Files = _downloadableFileConverter.ToFilesModels(userMedicament.Images!, FileCategory.UserMedicamentImage.IsPublic())
                }
        };
        return model;
    }

    private UserMedicamentSimpleModel ToModel(UserMedicamentSimple medicament)
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
}