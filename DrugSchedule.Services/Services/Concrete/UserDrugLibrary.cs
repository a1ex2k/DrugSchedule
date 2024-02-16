using DrugSchedule.StorageContract.Abstractions;
using DrugSchedule.StorageContract.Data;
using DrugSchedule.Services.Models;
using DrugSchedule.Services.Services.Abstractions;
using DrugSchedule.Services.Utils;
using OneOf.Types;
using DrugSchedule.Services.Converters;

namespace DrugSchedule.Services.Services;

public class UserDrugLibrary : IUserDrugLibrary
{
    private readonly ICurrentUserIdentifier _currentUserIdentifier;
    private readonly IUserDrugRepository _userDrugRepository;
    private readonly IReadonlyDrugRepository _drugRepository;
    private readonly IFileService _fileService;
    private readonly IUserMedicamentConverter _converter;
    private readonly IDownloadableFileConverter _downloadableFileConverter;

    public UserDrugLibrary(IFileService fileService, ICurrentUserIdentifier currentUserIdentifier,
        IUserDrugRepository userDrugRepository, IReadonlyDrugRepository drugRepository, IUserMedicamentConverter converter, IDownloadableFileConverter downloadableFileConverter)
    {
        _fileService = fileService;
        _currentUserIdentifier = currentUserIdentifier;
        _userDrugRepository = userDrugRepository;
        _drugRepository = drugRepository;
        _converter = converter;
        _downloadableFileConverter = downloadableFileConverter;
    }

    public async Task<OneOf<UserMedicamentExtendedModel, NotFound>> GetMedicamentExtendedAsync(long id,
        CancellationToken cancellationToken = default)
    {
        var medicament = await _userDrugRepository.GetMedicamentExtendedAsync(_currentUserIdentifier.UserProfileId, id,
            true, true, cancellationToken);

        if (medicament == null)
        {
            return new NotFound("Current user doesn't have custom medicament with provided ID");
        }

        if (medicament.BasicMedicamentId == null)
        {
            return _converter.ToUserMedicamentExtended(medicament, null);
        }

        var globalMedicament = await _drugRepository.GetMedicamentExtendedByIdAsync(medicament.BasicMedicamentId.Value, true, cancellationToken);
        return _converter.ToUserMedicamentExtended(medicament, globalMedicament);
    }


    public async Task<UserMedicamentExtendedCollection> GetMedicamentsExtendedAsync(UserMedicamentFilter filter, CancellationToken cancellationToken = default)
    {
        var medicaments = await _userDrugRepository.GetMedicamentsExtendedAsync(_currentUserIdentifier.UserProfileId, filter,
            true, true, cancellationToken);

        var basicIds = medicaments
            .Where(m => m.BasicMedicamentId != null)
            .Select(m => m.BasicMedicamentId!.Value)
            .ToList();

        var globalMedicaments = await _drugRepository.GetMedicamentsExtendedAsync(
            new MedicamentFilter { IdFilter = basicIds }, true, cancellationToken);

        return new UserMedicamentExtendedCollection
        {
            Medicaments = medicaments.ConvertAll(m =>
            {
                var gm = m.BasicMedicamentId == null
                    ? null
                    : globalMedicaments.Find(x => x.Id == m.BasicMedicamentId.Value);
                return _converter.ToUserMedicamentExtended(m, gm);
            })
        };
    }

    public async Task<OneOf<UserMedicamentSimpleModel, NotFound>> GetMedicamentSimpleAsync(long id, CancellationToken cancellationToken = default)
    {
        var medicament = await _userDrugRepository.GetMedicamentSimpleAsync(_currentUserIdentifier.UserProfileId, id, cancellationToken);
        if (medicament == null)
        {
            return new NotFound("Current user doesn't have custom medicament with provided ID");
        }

        return _converter.ToUserMedicamentSimple(medicament);
    }

    public async Task<UserMedicamentSimpleCollection> GetMedicamentsSimpleAsync(UserMedicamentFilter filter, CancellationToken cancellationToken = default)
    {
        var medicaments = await _userDrugRepository.GetMedicamentsSimpleAsync(_currentUserIdentifier.UserProfileId, filter, cancellationToken);
        return _converter.ToUserMedicamentSimpleCollection(medicaments);
    }

    public async Task<OneOf<UserMedicamentUpdate, InvalidInput>> CreateMedicamentAsync(NewUserMedicament model, CancellationToken cancellationToken = default)
    {
        var invalidInput = new InvalidInput();
        if (model.BasicMedicamentId != null)
        {
            var exists = await _drugRepository.DoesMedicamentExistAsync(model.BasicMedicamentId.Value, cancellationToken);
            if (!exists)
            {
                invalidInput.Add("Base medicament with provided ID not found");
            }
        }
        if (string.IsNullOrWhiteSpace(model.Name))
        {
            invalidInput.Add("Name must be non white space");
        }

        if (string.IsNullOrWhiteSpace(model.ReleaseForm))
        {
            invalidInput.Add("ReleaseForm must be non white space");
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
        return _converter.ToUpdateResultModel(savedMedicament!);
    }

    public async Task<OneOf<UserMedicamentUpdate, NotFound, InvalidInput>> UpdateMedicamentAsync(UserMedicamentUpdate model, CancellationToken cancellationToken = default)
    {
        var medicament =
            await _userDrugRepository.GetMedicamentAsync(_currentUserIdentifier.UserProfileId, model.Id,
                cancellationToken);
        if (medicament == null)
        {
            return new NotFound("Current user doesn't have custom medicament with provided ID");
        }

        var invalidInput = new InvalidInput();
        if (model.BasicMedicamentId != null && model.BasicMedicamentId != 0)
        {
            var exists = await _drugRepository.DoesMedicamentExistAsync(model.BasicMedicamentId.Value, cancellationToken);
            if (!exists)
            {
                invalidInput.Add("Base medicament with provided ID not found. To remove, set BasicMedicamentId=0");
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
        return _converter.ToUpdateResultModel(savedMedicament!);
    }

    public async Task<OneOf<True, NotFound, InvalidInput>> RemoveMedicamentAsync(long id, CancellationToken cancellationToken = default)
    {
        var deleteResult = await _userDrugRepository.RemoveMedicamentAsync(_currentUserIdentifier.UserProfileId, id, cancellationToken);
        switch (deleteResult)
        {
            case RemoveOperationResult.Removed:
                return new True();
            case RemoveOperationResult.Used:
                return new InvalidInput("User medicament cannot be removed because it is referenced by another object");
            default:
                return new NotFound("User medicament with provided ID not found");
        }
    }


    public async Task<OneOf<DownloadableFile, NotFound, InvalidInput>> AddImageAsync(long medicamentId, InputFile inputFile, CancellationToken cancellationToken = default)
    {
        var medicament =
            await _userDrugRepository.GetMedicamentAsync(_currentUserIdentifier.UserProfileId, medicamentId, cancellationToken);
        if (medicament == null)
        {
            return new NotFound("Current user doesn't have custom medicament with provided ID");
        }

        var addResult = await CreateFileAsync(inputFile, cancellationToken);
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
            return new NotFound("Current user doesn't have custom medicament with provided ID");
        }

        if (medicament.ImageGuids?.Contains(fileGuid) != true)
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

    private async Task<OneOf<FileInfo, InvalidInput>> CreateFileAsync(InputFile inputFile, CancellationToken cancellationToken)
    {
        return await _fileService.CreateAsync(
            inputFile,
            FileCategory.UserMedicamentImage.GetAwaitableParams(),
            FileCategory.UserMedicamentImage,
            cancellationToken);
    }
}