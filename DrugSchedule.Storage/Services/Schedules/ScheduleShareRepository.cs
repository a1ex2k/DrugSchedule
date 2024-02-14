using DrugSchedule.Storage.Data;
using DrugSchedule.Storage.Extensions;
using DrugSchedule.StorageContract.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DrugSchedule.Storage.Services;

public class ScheduleShareRepository : IScheduleShareRepository
{
    private readonly DrugScheduleContext _dbContext;
    private readonly ILogger<ScheduleShareRepository> _logger;

    public ScheduleShareRepository(DrugScheduleContext dbContext, ILogger<ScheduleShareRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Contract.ScheduleSharePlain?> GetScheduleShareAsync(long id, CancellationToken cancellationToken = default)
    {
        var share = await _dbContext.ScheduleShare
            .AsNoTracking()
            .Select(EntityMapExpressions.ToScheduleSharePlain)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        return share;
    }

    public async Task<List<Contract.ScheduleSharePlain>> GetScheduleSharesAsync(long scheduleId, CancellationToken cancellationToken = default)
    {
        var shares = await _dbContext.ScheduleShare
            .AsNoTracking()
            .Where(s => s.MedicamentTakingScheduleId == scheduleId)
            .Select(EntityMapExpressions.ToScheduleSharePlain)
            .ToListAsync(cancellationToken);
        return shares;
    }

    public async Task<List<Contract.ScheduleSharePlain>> GetSharesWithContactAsync(long contactUserId, CancellationToken cancellationToken = default)
    {
        var shares = await _dbContext.ScheduleShare
            .AsNoTracking()
            .Where(s => s.ShareWithContact!.ContactProfileId == contactUserId)
            .Select(EntityMapExpressions.ToScheduleSharePlain)
            .ToListAsync(cancellationToken);
        return shares;
    }

    public async Task<Contract.ScheduleSharePlain?> AddOrUpdateShareAsync(Contract.ScheduleSharePlain scheduleShare, CancellationToken cancellationToken = default)
    {
        var share = await _dbContext.ScheduleShare
            .Include(s => s.ShareWithContact!.ContactProfileId)
            .FirstOrDefaultAsync(s => s.Id == scheduleShare.Id 
                                      && s.MedicamentTakingScheduleId == scheduleShare.MedicamentTakingScheduleId,
                cancellationToken);

        var contactId = await _dbContext.UserProfileContacts
            .Where(c => c.ContactProfileId == scheduleShare.ShareUserProfileId)
            .Where(c => _dbContext.MedicamentTakingSchedules
                .Where(m => m.Id == scheduleShare.MedicamentTakingScheduleId)
                .Select(m => m.UserProfileId)
                .Contains(c.UserProfileId))
            .Select(c => c.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (contactId == default) return null;

        if (share == null || contactId != share.ShareWithContactId)
        {
            share = new Entities.ScheduleShare
            {
                Id = 0,
                MedicamentTakingScheduleId = scheduleShare.MedicamentTakingScheduleId,
                ShareWithContactId = contactId,
                Comment = scheduleShare.Comment,
            };
            await _dbContext.ScheduleShare.AddAsync(share, cancellationToken);
        }
        else
        {
            share.Comment = scheduleShare.Comment;
        }

        var saved = await _dbContext.TrySaveChangesAsync(_logger, cancellationToken);
        return saved
            ? new Contract.ScheduleSharePlain
            {
                Id = share.Id,
                MedicamentTakingScheduleId = share.MedicamentTakingScheduleId,
                ShareUserProfileId = scheduleShare.ShareUserProfileId,
                Comment = share.Comment,
            }
            : null;
    }

    public async Task<Contract.RemoveOperationResult> RemoveTakingScheduleAsync(long id, CancellationToken cancellationToken = default)
    {
        var deleted = await _dbContext.ScheduleShare
            .Where(c => c.Id ==  id)
            .ExecuteDeleteAsync(cancellationToken);
        return deleted == 1 ? Contract.RemoveOperationResult.Removed : Contract.RemoveOperationResult.NotFound;
    }
}