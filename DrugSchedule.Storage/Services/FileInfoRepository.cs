using DrugSchedule.Storage.Extensions;
using DrugSchedule.Storage.Data;
using DrugSchedule.StorageContract.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DrugSchedule.Storage.Services;

public class FileInfoRepository : IFileInfoRepository
{
    private readonly DrugScheduleContext _dbContext;
    private readonly ILogger<FileInfoRepository> _logger;

    public FileInfoRepository(DrugScheduleContext dbContext, UserManager<IdentityUser> userManager,
        ILogger<FileInfoRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Contract.FileInfo?> GetFileInfoAsync(Guid fileGuid, CancellationToken cancellationToken = default)
    {
        var info = await _dbContext.FileInfos
            .Select(EntityMapExpressions.ToFileInfo)
            .FirstOrDefaultAsync(i => i.Guid == fileGuid, cancellationToken);
        return info;
    }

    public async Task<List<Contract.FileInfo>> GetFileInfosAsync(List<Guid> guids,
        CancellationToken cancellationToken = default)
    {
        var infoList = await _dbContext.FileInfos
            .Where(i => guids.Contains(i.Guid))
            .Select(EntityMapExpressions.ToFileInfo)
            .ToListAsync(cancellationToken);
        return infoList;
    }

    public async Task<Contract.FileInfo?> AddFileInfoAsync(Contract.FileInfo fileInfo,
        CancellationToken cancellationToken = default)
    {
        var newFileInfo = new Entities.FileInfo
        {
            Guid = fileInfo.Guid,
            OriginalName = fileInfo.OriginalName,
            Extension = fileInfo.FileExtension,
            FileCategory = fileInfo.Category,
            MediaType = fileInfo.MediaType,
            Size = fileInfo.Size,
            CreatedAt = fileInfo.CreatedAt,
            HasThumbnail = fileInfo.HasThumbnail
        };

        await _dbContext.FileInfos.AddAsync(newFileInfo, cancellationToken);
        return await _dbContext.TrySaveChangesAsync(_logger, cancellationToken) ? newFileInfo.ToContractModel() : null;
    }

    public async Task<Contract.RemoveOperationResult> RemoveFileByGuidAsync(Guid guid,
        CancellationToken cancellationToken = default)
    {
        var info = await _dbContext.FileInfos
            .FirstOrDefaultAsync(i => i.Guid == guid, cancellationToken);
        if (info == null)
        {
            return Contract.RemoveOperationResult.NotFound;
        }

        _dbContext.FileInfos.Remove(info);

        var removed = await _dbContext.TrySaveChangesAsync(_logger, cancellationToken);
        return removed ? Contract.RemoveOperationResult.Removed : Contract.RemoveOperationResult.Used;
    }
}