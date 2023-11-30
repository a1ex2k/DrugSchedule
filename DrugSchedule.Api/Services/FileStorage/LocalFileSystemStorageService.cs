namespace DrugSchedule.Api.Services.FileStorage;

public class LocalFileSystemStorageService : IFileStorageService
{
    private readonly ILogger<LocalFileSystemStorageService> _logger;


    public async Task<FileOrError> GetFileByGuidAsync(Guid guid)
    {
        throw new NotImplementedException();
    }

    public async Task<FileOrError> AddNewFileAsync(NewFile file)
    {
        throw new NotImplementedException();
    }

    public async Task<RemovedFileOrError> RemoveFileByGuidAsync(Guid guid)
    {
        throw new NotImplementedException();
    }
}