namespace DrugSchedule.Api.Services.FileStorage;

public interface IFileStorageService
{
    Task<FileOrError> GetFileByGuidAsync(Guid guid);

    Task<FileOrError> AddNewFileAsync(NewFile file);

    Task<RemovedFileOrError> RemoveFileByGuidAsync(Guid guid);
}