using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.StorageContract.Abstractions;

public interface IFileInfoRepository
{
    Task<FileInfo?> GetFileInfoAsync(Guid fileGuid, CancellationToken cancellationToken = default);

    Task<List<FileInfo>> GetFileInfosAsync(List<Guid> guids, CancellationToken cancellationToken = default);

    Task<FileInfo?> AddFileInfoAsync(FileInfo newFileInfo, CancellationToken cancellationToken = default);

    Task<RemoveOperationResult> RemoveFileByGuidAsync(Guid fileGuid, CancellationToken cancellationToken = default);
}