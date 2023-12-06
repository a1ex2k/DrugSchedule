using System;
using System.IO;
using System.Threading.Tasks;
using DrugSchedule.StorageContract.Data.FileStorage;
using File = DrugSchedule.StorageContract.Data.FileStorage.File;
using FileInfo = DrugSchedule.StorageContract.Data.FileStorage.FileInfo;

namespace DrugSchedule.StorageContract.Abstractions;

public interface IFileStorage : IRemoteFileUrlProvider
{
    Task<File> GetFileByGuidAsync(Guid guid);

    Task<FileInfo> GetFileInfoByGuidAsync(Guid guid);

    Task<FileInfo> CreateFileAsync(NewFileInfo newFileInfo, Stream stream);

    Task<RemovedFileInfo> RemoveFileByGuidAsync(Guid guid);
}