using DrugSchedule.BusinessLogic.Models;
using DrugSchedule.BusinessLogic.Utils;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.BusinessLogic.Services;

public interface IFileService
{
    Task<OneOf<FileInfo, NotFound>> GetFileInfoAsync(Guid fileGuid, CancellationToken cancellationToken = default);

    Task<List<FileInfo>> GetFileInfosAsync(List<Guid> fileGuids, CancellationToken cancellationToken = default);

    Task<OneOf<FileData, NotFound>> GetFileDataAsync(Guid fileGuid, CancellationToken cancellationToken = default);
    
    Task<OneOf<FileInfo, InvalidInput>> CreateAsync(InputFile inputFile, AwaitableFileParams awaitableFileParams, FileCategory category, CancellationToken cancellationToken = default);

    Task<OneOf<bool, NotFound>> RemoveFileAsync(Guid fileGuid, CancellationToken cancellationToken = default);
}