using DrugSchedule.Services.Errors;
using DrugSchedule.Services.Models;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Services.Services;

public interface IFileService
{
    Task<OneOf<FileInfo, NotFound>> GetFileInfoAsync(Guid fileGuid, CancellationToken cancellationToken = default);

    Task<List<FileInfo>> GetFileInfosAsync(List<Guid> fileGuids, CancellationToken cancellationToken = default);

    Task<OneOf<FileData, NotFound>> GetFileDataAsync(Guid fileGuid, CancellationToken cancellationToken = default);

    Task<OneOf<FileData, NotFound>> GetThumbnailAsync(Guid fileGuid, CancellationToken cancellationToken = default);
            
    Task<OneOf<FileInfo, InvalidInput>> CreateAsync(InputFile inputFile, AwaitableFileParams awaitableFileParams, FileCategory category, CancellationToken cancellationToken = default);

    Task<OneOf<bool, NotFound>> RemoveFileAsync(Guid fileGuid, CancellationToken cancellationToken = default);
}