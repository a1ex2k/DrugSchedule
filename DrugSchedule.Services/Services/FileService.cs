using DrugSchedule.BusinessLogic.Models;
using DrugSchedule.BusinessLogic.Services.Abstractions;
using DrugSchedule.BusinessLogic.Utils;
using DrugSchedule.StorageContract.Abstractions;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.BusinessLogic.Services;

public class FileService : IFileService
{
    private readonly IFileInfoRepository _fileInfoRepository;
    private readonly IFileStorage _fileStorage;
    private readonly IFileUrlProvider _fileUrlProvider;

    public FileService(IFileInfoRepository fileInfoRepository, IFileStorage fileStorage, IFileUrlProvider fileUrlProvider)
    {
        _fileInfoRepository = fileInfoRepository;
        _fileStorage = fileStorage;
        _fileUrlProvider = fileUrlProvider;
    }

    public async Task<OneOf<FileInfo, NotFound>> GetFileInfoAsync(Guid fileGuid, CancellationToken cancellationToken = default)
    {
        var fileInfo = await _fileInfoRepository.GetFileInfoAsync(fileGuid, cancellationToken);
        if (fileInfo == null)
        {
            return new NotFound("DownloadableFile info not found");
        }

        return fileInfo;
    }

    public async Task<List<FileInfo>> GetFileInfosAsync(List<Guid> fileGuids, CancellationToken cancellationToken = default)
    {
        var fileInfos = await _fileInfoRepository.GetFileInfosAsync(fileGuids, cancellationToken);
        return fileInfos;
    }


    public Task<List<DownloadableFile>> GetDownloadableFilesAsync(List<Guid> fileGuids, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<OneOf<FileData, NotFound>> GetFileDataAsync(Guid fileGuid, CancellationToken cancellationToken = default)
    {
        if (fileGuid == Guid.Empty)
        {
            return new NotFound("Empty guid");
        }
        
        var fileInfo = await _fileInfoRepository.GetFileInfoAsync(fileGuid, cancellationToken);
        if (fileInfo == null)
        {
            return new NotFound("DownloadableFile not found");
        }
        
        var stream = await _fileStorage.GetReadStreamAsync(fileInfo, cancellationToken);
        if (stream == null)
        {
            return new NotFound("DownloadableFile not found");
        }

        var fileData = new FileData
        {
            FileInfo = fileInfo,
            Stream = stream
        };
        return fileData;
    }

    public async Task<OneOf<FileInfo, InvalidInput>> CreateAsync(NewCategorizedFile newCategorizedFile, CancellationToken cancellationToken = default)
    {
        var error = new InvalidInput();
        if (string.IsNullOrWhiteSpace(newCategorizedFile.MediaType))
        {
            error.Add("Media type (a.k.a. mime type) not set");
        }

        if (string.IsNullOrWhiteSpace(newCategorizedFile.NameWithExtension))
        {
            error.Add("Original filename not set");
        }

        if (newCategorizedFile.Stream == null || !newCategorizedFile.Stream.CanRead || newCategorizedFile.Stream.Length == 0)
        {
            error.Add("Input data stream cannot be read");
        }

        if (error.HasMessages)
        {
            return error;
        }

        var fileName = Path.GetFileNameWithoutExtension(newCategorizedFile.NameWithExtension);
        var extension = newCategorizedFile.NameWithExtension.Substring(fileName.Length);
        var fileInfo = new FileInfo
        {
            Guid = Guid.NewGuid(),
            OriginalName = fileName ?? Path.GetRandomFileName(),
            FileExtension = string.IsNullOrWhiteSpace(fileName) ? string.Empty : extension,
            Category = newCategorizedFile.Category,
            MediaType = newCategorizedFile.MediaType,
            Size = newCategorizedFile.Stream!.Length,
            CreatedAt = DateTime.UtcNow
        };
        var dataWritten = await _fileStorage.WriteFileAsync(fileInfo, newCategorizedFile.Stream!, cancellationToken);
        var fileInfoSaved = await _fileInfoRepository.AddFileInfoAsync(fileInfo, cancellationToken);
        return fileInfo;
    }

    public async Task<OneOf<bool, NotFound>> RemoveFileAsync(Guid fileGuid, CancellationToken cancellationToken = default)
    {
        var fileInfo = await _fileInfoRepository.GetFileInfoAsync(fileGuid, cancellationToken);
        if (fileInfo == null)
        {
            return new NotFound("File info not found");
        }

        var wasRemovedFromStorage = await _fileStorage.RemoveFileAsync(fileInfo, cancellationToken);
        if (!wasRemovedFromStorage)
        {
            return false;
        }

        var infoRemoveResult = await _fileInfoRepository.RemoveFileByGuidAsync(fileGuid, cancellationToken);
        
        return wasRemovedFromStorage && infoRemoveResult == RemoveOperationResult.Removed;
    }
}