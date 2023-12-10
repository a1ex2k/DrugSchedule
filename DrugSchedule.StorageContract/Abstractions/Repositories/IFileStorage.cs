using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DrugSchedule.StorageContract.Data;
using File = DrugSchedule.StorageContract.Data.File;
using FileInfo = DrugSchedule.StorageContract.Data.FileInfo;

namespace DrugSchedule.StorageContract.Abstractions;

public interface IFileStorage : IRemoteFileUrlProvider
{
    Task<File> GetFileByGuidAsync(Guid guid);

    Task<FileInfo> GetFileInfoByGuidAsync(Guid guid);

    Task<FileInfo> CreateFileInfoAsync(NewFileInfo newFileInfo);

    Task<FileInfo> CreateFileAsync(NewFileInfo newFileInfo, Stream stream);

    Task<RemovedFileInfo> RemoveFileByGuidAsync(Guid guid);
}