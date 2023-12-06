using System;
using System.Threading.Tasks;
using DrugSchedule.StorageContract.Data.FileStorage;

namespace DrugSchedule.StorageContract.Abstractions;

public interface IRemoteFileUrlProvider
{
    Task<FileUrl> GetFileDownloadUrlByGuidAsync(Guid guid);

    Task<FileUrl> GetFileUploadUrlByGuidAsync(Guid guid);
}