using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FileInfo = DrugSchedule.StorageContract.Data.FileInfo;

namespace DrugSchedule.StorageContract.Abstractions;

public interface IFileStorage
{
    Task<Stream?> GetReadStreamAsync(FileInfo fileInfo, CancellationToken cancellationToken = default);

    Task<bool> WriteFileAsync(FileInfo fileInfo, Stream stream, CancellationToken cancellationToken = default);

    Task<bool> RemoveFileAsync(FileInfo fileInfo, CancellationToken cancellationToken = default);
}