using DrugSchedule.Storage.Extensions;
using DrugSchedule.Storage.Data;
using DrugSchedule.StorageContract.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserProfile = DrugSchedule.Storage.Data.Entities.UserProfile;
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


    public async Task<UserMedicamentExtended?> GetMedicamentsExtendedAsync(long userProfileId, long id, bool withImages, bool withBasicMedicament,
        CancellationToken cancellationToken = default)
    {
        var medicament = await _dbContext.UserMedicaments
            .AsNoTracking()
            .Where(m => m.Id == id && m.UserProfileId == userProfileId)
            .Select(EntityMapExpressions.ToUserMedicamentExtended(withImages, withBasicMedicament))
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
            .WithFilterOrIfNull(m => m.ReleaseForm,
                m => m.BasedOnMedicament!.ReleaseForm!.Name,
                filter.ReleaseFormNameFilter)
            .WithFilterOrIfNull(m => m.ManufacturerName,
                m => m.BasedOnMedicament!.Manufacturer!.Name,
                filter.ManufacturerNameFilter)
            .Select(EntityMapExpressions.ToUserMedicamentExtended(withImages, withBasicMedicament))
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
            .WithFilterOrIfNull(m => m.ReleaseForm,
                m => m.BasedOnMedicament!.ReleaseForm!.Name,
                filter.ReleaseFormNameFilter)
            .WithFilterOrIfNull(m => m.ManufacturerName,
                m => m.BasedOnMedicament!.Manufacturer!.Name,
                filter.ManufacturerNameFilter)
            .Select(EntityMapExpressions.ToUserMedicamentSimple)
            .ToListAsync(cancellationToken);
        return medicaments;
    }

    public Task<UserMedicamentExtended?> CreateMedicamentAsync(UserMedicamentExtended userMedicamentExtended,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<UserMedicamentExtended?> UpdateMedicamentAsync(UserMedicamentExtended userMedicamentExtended, UserMedicamentUpdateFlags updateFlags,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<RemoveOperationResult> RemoveContactAsync(long medicamentId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}