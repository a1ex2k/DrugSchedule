using DrugSchedule.Storage.Extensions;
using DrugSchedule.Storage.Data;
using DrugSchedule.StorageContract.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DrugSchedule.StorageContract.Data;


namespace DrugSchedule.Storage.Services;

public class UserDrugRepository : IUserDrugRepository
{
    private readonly DrugScheduleContext _dbContext;
    private readonly ILogger<UserDrugRepository> _logger;

    public UserDrugRepository(DrugScheduleContext dbContext, ILogger<UserDrugRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }


    public async Task<UserMedicamentExtended?> GetMedicamentExtendedAsync(long userProfileId, long id, bool withImages, bool withBasicMedicament,
        CancellationToken cancellationToken = default)
    {
        var medicament = await _dbContext.UserMedicaments
            .AsNoTracking()
            .Where(m => m.Id == id && m.UserProfileId == userProfileId)
            .Select(EntityMapExpressions.ToUserMedicamentExtended(withImages))
            .FirstOrDefaultAsync(cancellationToken);
        return medicament;
    }

    public async Task<List<UserMedicamentExtended>> GetMedicamentsExtendedAsync(long userProfileId, UserMedicamentFilter filter, bool withImages,
        bool withBasicMedicament, CancellationToken cancellationToken = default)
    {
        var medicaments = await _dbContext.UserMedicaments
            .AsNoTracking()
            .Where(m => m.UserProfileId == userProfileId)
            .WithFilter(m => m.Id, filter.IdFilter)
            .WithFilter(m => m.Name, filter.NameFilter)
            .WithFilter(m => m.ReleaseForm, filter.ReleaseFormNameFilter)
            .WithFilter(m => m.ManufacturerName!, filter.ManufacturerNameFilter)
            .Select(EntityMapExpressions.ToUserMedicamentExtended(withImages))
            .ToListAsync(cancellationToken);
        return medicaments;
    }

    public async Task<UserMedicamentSimple?> GetMedicamentSimpleAsync(long userProfileId, long id, CancellationToken cancellationToken = default)
    {
        var medicament = await _dbContext.UserMedicaments
            .AsNoTracking()
            .Where(m => m.Id == id && m.UserProfileId == userProfileId)
            .Select(EntityMapExpressions.ToUserMedicamentSimple)
            .FirstOrDefaultAsync(cancellationToken);
        return medicament;
    }

    public async Task<List<UserMedicamentSimple>> GetMedicamentsSimpleAsync(long userProfileId, UserMedicamentFilter filter, CancellationToken cancellationToken = default)
    {
        var medicaments = await _dbContext.UserMedicaments
            .AsNoTracking()
            .Where(m => m.UserProfileId == userProfileId)
            .WithFilter(m => m.Id, filter.IdFilter)
            .WithFilter(m => m.Name, filter.NameFilter)
            .WithFilter(m => m.ReleaseForm, filter.ReleaseFormNameFilter)
            .WithFilter(m => m.ManufacturerName!, filter.ManufacturerNameFilter)
            .Select(EntityMapExpressions.ToUserMedicamentSimple)
            .ToListAsync(cancellationToken);
        return medicaments;
    }

    public async Task<Contract.UserMedicament?> GetMedicamentAsync(long userProfileId, long id, CancellationToken cancellationToken = default)
    {
        var medicament = await _dbContext.UserMedicaments
            .AsNoTracking()
            .Where(u => u.Id == userProfileId && u.UserProfileId == userProfileId)
            .Select(EntityMapExpressions.ToUserMedicament)
            .FirstOrDefaultAsync(cancellationToken);
        return medicament;
    }

    public async Task<Contract.UserMedicament?> CreateMedicamentAsync(Contract.UserMedicament model, CancellationToken cancellationToken = default)
    {
        var entity = new Entities.UserMedicament
        {
            BasedOnMedicamentId = model.BasicMedicamentId,
            Name = model.Name,
            Description = model.Description,
            Composition = model.Composition,
            ReleaseForm = model.ReleaseForm,
            ManufacturerName = model.ManufacturerName,
            UserProfileId = model.UserProfileId,
        };

        if (!model.ImageGuids.IsNullOrEmpty())
        {
            entity.Files = model.ImageGuids!.ConvertAll(g => new Entities.UserMedicamentFile { FileGuid = g });
        }

        await _dbContext.UserMedicaments.AddAsync(entity, cancellationToken);
        var saved = await _dbContext.TrySaveChangesAsync(_logger, cancellationToken);
        return saved ? entity.ToContractModel() : null;
    }

    public async Task<Contract.UserMedicament?> UpdateMedicamentAsync(Contract.UserMedicament medicament, UserMedicamentUpdateFlags updateFlags, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.UserMedicaments
            .Where(m => m.Id == medicament.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null) return null;

        var entry = _dbContext.Entry(entity);
        entry.UpdateIf(e => e.Name, medicament.Name, updateFlags.Name);
        entry.UpdateIf(e => e.BasedOnMedicamentId, medicament.BasicMedicamentId, updateFlags.BasedOnMedicament);
        entry.UpdateIf(e => e.ReleaseForm, medicament.ReleaseForm, updateFlags.ReleaseForm);
        entry.UpdateIf(e => e.ManufacturerName, medicament.ManufacturerName, updateFlags.ManufacturerName);
        entry.UpdateIf(e => e.Composition, medicament.Composition, updateFlags.Composition);
        entry.UpdateIf(e => e.Description, medicament.Description, updateFlags.Description);

        if (updateFlags.Images)
        {
            entity.Files.RemoveAndAddExceptExistingByKey(medicament.ImageGuids,
                existing => existing.FileGuid, guid => guid,
                guid => new Entities.UserMedicamentFile() { FileGuid = guid });
        }

        await _dbContext.UserMedicaments.AddAsync(entity, cancellationToken);
        var saved = await _dbContext.TrySaveChangesAsync(_logger, cancellationToken);
        return saved ? entity.ToContractModel() : null;
    }

    public async Task<RemoveOperationResult> RemoveMedicamentAsync(long userProfileId, long id, CancellationToken cancellationToken = default)
    {
        var existing = await _dbContext.UserMedicaments
            .FirstOrDefaultAsync(m => m.Id == id && m.UserProfileId == userProfileId, cancellationToken);

        if (existing == null)
        {
            return RemoveOperationResult.NotFound;
        }

        _dbContext.UserMedicaments.Remove(existing);
        var removed = await _dbContext.TrySaveChangesAsync(_logger, cancellationToken);
        return removed ? RemoveOperationResult.Removed : RemoveOperationResult.Used;
    }
}