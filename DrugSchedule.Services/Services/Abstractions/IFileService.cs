using DrugSchedule.BusinessLogic.Models;
using DrugSchedule.BusinessLogic.Utils;

namespace DrugSchedule.BusinessLogic.Services;

public interface IFileService
{
    Task<OneOf<FileInfo, NotFound>> GetFileInfoAsync(Guid fileGuid, CancellationToken cancellationToken = default);

    Task<List<FileInfo>> GetFileInfosAsync(List<Guid> fileGuids, CancellationToken cancellationToken = default);

    Task<OneOf<FileData, NotFound>> GetFileDataAsync(Guid fileGuid, CancellationToken cancellationToken = default);
    
    Task<OneOf<FileInfo, InvalidInput>> CreateAsync(NewCategorizedFile newCategorizedFileInfoModel, CancellationToken cancellationToken = default);

    Task<OneOf<bool, NotFound>> RemoveFileInfoAsync(Guid fileGuid, CancellationToken cancellationToken = default);
}