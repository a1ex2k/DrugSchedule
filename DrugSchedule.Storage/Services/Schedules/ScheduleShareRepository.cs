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

    public async Task<List<Contract.ScheduleSharePlain>> GetScheduleSharesAsync(long scheduleId,
        CancellationToken cancellationToken = default)
    {
        var shares = await _dbContext.ScheduleShare
            .Where(s => s.MedicamentTakingScheduleId == scheduleId)
            .Select(EntityMapExpressions.ToScheduleSharePlain)
            .ToListAsync(cancellationToken);
        return shares;
    }

    public async Task<Contract.ScheduleSharePlain?> AddOrUpdateShareAsync(Contract.ScheduleSharePlain scheduleShare,
        CancellationToken cancellationToken = default)
    {
        var share = await _dbContext.ScheduleShare
            .Include(s => s.ShareWithContact!.ContactProfileId)
            .Where(s => s.MedicamentTakingScheduleId == scheduleShare.ScheduleId)
            .Where(s => s.ShareWithContact!.ContactProfileId == scheduleShare.ShareUserProfileId)
            .FirstOrDefaultAsync(cancellationToken);

        var contactId = await _dbContext.UserProfileContacts
            .Where(c => c.ContactProfileId == scheduleShare.ShareUserProfileId)
            .Where(c => _dbContext.MedicamentTakingSchedules
                .Where(m => m.Id == scheduleShare.ScheduleId)
                .Select(m => m.UserProfileId)
                .Contains(c.UserProfileId))
            .Select(c => c.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (contactId == default) return null;

        if (share == null || contactId != share.ShareWithContactId)
        {
            share = new Entities.ScheduleShare
            {
                MedicamentTakingScheduleId = scheduleShare.ScheduleId,
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
                ScheduleId = share.MedicamentTakingScheduleId,
                ShareUserProfileId = scheduleShare.ShareUserProfileId,
                Comment = share.Comment,
            }
            : null;
    }

    public async Task<Contract.RemoveOperationResult> RemoveScheduleShareAsync(long scheduleId, long contactProfileId,
        CancellationToken cancellationToken = default)
    {
        var deleted = await _dbContext.ScheduleShare
            .Where(s => s.MedicamentTakingScheduleId == scheduleId)
            .Where(s => s.ShareWithContact!.ContactProfileId == contactProfileId)
            .ExecuteDeleteAsync(cancellationToken);
        return deleted == 1 ? Contract.RemoveOperationResult.Removed : Contract.RemoveOperationResult.NotFound;
    }
}