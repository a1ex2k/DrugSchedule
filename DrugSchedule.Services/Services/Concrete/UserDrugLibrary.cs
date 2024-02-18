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
    private readonly ISharedDataRepository _sharedRepository;
    private readonly IFileService _fileService;
    private readonly IUserMedicamentConverter _converter;
    private readonly IDownloadableFileConverter _downloadableFileConverter;

    public UserDrugLibrary(IFileService fileService, ICurrentUserIdentifier currentUserIdentifier,
        IUserDrugRepository userDrugRepository, IReadonlyDrugRepository drugRepository, IUserMedicamentConverter converter, IDownloadableFileConverter downloadableFileConverter, ISharedDataRepository sharedRepository)
    {
        _fileService = fileService;
        _currentUserIdentifier = currentUserIdentifier;
        _userDrugRepository = userDrugRepository;
        _drugRepository = drugRepository;
        _converter = converter;
        _downloadableFileConverter = downloadableFileConverter;
        _sharedRepository = sharedRepository;
    }

    public async Task<OneOf<UserMedicamentExtendedModel, NotFound>> GetMedicamentExtendedAsync(long id,
        CancellationToken cancellationToken = default)
    {
        var medicament = await _userDrugRepository.GetMedicamentExtendedAsync(_currentUserIdentifier.UserId, id,
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
        var medicaments = await _userDrugRepository.GetMedicamentsExtendedAsync(_currentUserIdentifier.UserId, filter,
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
        var medicament = await _userDrugRepository.GetMedicamentSimpleAsync(_currentUserIdentifier.UserId, id, cancellationToken);
        if (medicament == null)
        {
            return new NotFound("Current user doesn't have custom medicament with provided ID");
        }

        return _converter.ToUserMedicamentSimple(medicament);
    }

    public async Task<UserMedicamentSimpleCollection> GetMedicamentsSimpleAsync(UserMedicamentFilter filter, CancellationToken cancellationToken = default)
    {
        var medicaments = await _userDrugRepository.GetMedicamentsSimpleAsync(_currentUserIdentifier.UserId, filter, cancellationToken);
        return _converter.ToUserMedicamentSimpleCollection(medicaments);
    }

    public async Task<OneOf<UserMedicamentId, InvalidInput>> CreateMedicamentAsync(NewUserMedicament model, CancellationToken cancellationToken = default)
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

        var medicament = new UserMedicamentPlain
        {
            UserId = _currentUserIdentifier.UserId,
            BasicMedicamentId = model.BasicMedicamentId,
            Name = model.Name.Trim(),
            ReleaseForm = model.ReleaseForm.Trim(),
            Description = string.IsNullOrWhiteSpace(model.Description) ? null : model.Description.Trim(),
            Composition = string.IsNullOrWhiteSpace(model.Composition) ? null : model.Composition.Trim(),
            ManufacturerName = string.IsNullOrWhiteSpace(model.ManufacturerName) ? null : model.ManufacturerName.Trim(),
        };

        var savedMedicament = await _userDrugRepository.CreateMedicamentAsync(medicament, cancellationToken);
        return (UserMedicamentId)savedMedicament!.Id;
    }

    public async Task<OneOf<UserMedicamentId, NotFound, InvalidInput>> UpdateMedicamentAsync(UserMedicamentUpdate model, CancellationToken cancellationToken = default)
    {
        var medicamentExists = await _userDrugRepository.DoesMedicamentExistAsync(model.Id, _currentUserIdentifier.UserId, cancellationToken);
        if (medicamentExists)
        {
            return new NotFound("Current user doesn't have custom medicament with provided ID");
        }

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

        var updateFlags = new UserMedicamentUpdateFlags
        {
            BasedOnMedicament = true,
            Name = true,
            Description = true,
            Composition = true,
            ReleaseForm = true,
            ManufacturerName = true,
        };

        var medicament = new UserMedicamentPlain
        {
            BasicMedicamentId = model.BasicMedicamentId,
            Name = model.Name.Trim(),
            ReleaseForm = model.ReleaseForm.Trim(),
            Description = model.Description?.Trim(),
            Composition = model.Composition?.Trim(),
            ManufacturerName = model.ManufacturerName?.Trim(),
        };

        var savedMedicament = await _userDrugRepository.UpdateMedicamentAsync(medicament, updateFlags, cancellationToken);
        return (UserMedicamentId)savedMedicament!.Id;
    }

    public async Task<OneOf<True, NotFound, InvalidInput>> RemoveMedicamentAsync(long id, CancellationToken cancellationToken = default)
    {
        var deleteResult = await _userDrugRepository.RemoveMedicamentAsync(_currentUserIdentifier.UserId, id, cancellationToken);
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
        var medicamentExists = await _userDrugRepository.DoesMedicamentExistAsync(_currentUserIdentifier.UserId, medicamentId, cancellationToken);
        if (!medicamentExists)
        {
            return new NotFound("Current user doesn't have custom medicament with provided ID");
        }

        var addResult = await CreateFileAsync(inputFile, cancellationToken);
        if (addResult.IsT1)
        {
            return addResult.AsT1;
        }

        _ = await _userDrugRepository.AddMedicamentImageAsync(medicamentId, addResult.AsT0.Guid, cancellationToken);
        return _downloadableFileConverter.ToFileModel(addResult.AsT0, true)!;
    }

    public async Task<OneOf<True, NotFound>> RemoveImageAsync(long medicamentId, Guid fileGuid, CancellationToken cancellationToken = default)
    {
        var medicamentExists = await _userDrugRepository.DoesMedicamentExistAsync(_currentUserIdentifier.UserId, medicamentId, cancellationToken);
        if (!medicamentExists)
        {
            return new NotFound("Current user doesn't have custom medicament with provided ID");
        }

        var removeResult = await _userDrugRepository.RemoveMedicamentImageAsync(medicamentId, fileGuid, cancellationToken);
        if (removeResult != RemoveOperationResult.Removed)
        {
            return new NotFound($"User medicament doesn't contain any image with provided Guid");
        }
        
        return new True();
    }

    public async Task<OneOf<UserMedicamentExtendedModel, NotFound>> GetSharedUserMedicamentAsync(long userMedicamentId, CancellationToken cancellationToken = default)
    {
        var medicament = await _sharedRepository.GetSharedUserMedicament(userMedicamentId,
            _currentUserIdentifier.UserId, cancellationToken);

        if (medicament == null)
        {
            return new NotFound("Shared user medicament was not found or current user doesn't have permissions to access");
        }

        if (medicament.BasicMedicamentId == null)
        {
            return _converter.ToUserMedicamentExtended(medicament, null);
        }

        var globalMedicament = await _drugRepository.GetMedicamentExtendedByIdAsync(medicament.BasicMedicamentId.Value, true, cancellationToken);

        return _converter.ToUserMedicamentExtended(medicament, globalMedicament);
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