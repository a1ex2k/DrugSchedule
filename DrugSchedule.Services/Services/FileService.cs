using DrugSchedule.BusinessLogic.Models;
using DrugSchedule.BusinessLogic.Utils;
using DrugSchedule.StorageContract.Abstractions;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.BusinessLogic.Services;

public class FileService : IFileService
{
    private readonly IFileInfoRepository _fileInfoRepository;
    private readonly IFileStorage _fileStorage;

    public FileService(IFileInfoRepository fileInfoRepository, IFileStorage fileStorage)
    {
        _fileInfoRepository = fileInfoRepository;
        _fileStorage = fileStorage;
    }

    public async Task<OneOf<FileInfo, NotFound>> GetFileInfoAsync(Guid fileGuid, CancellationToken cancellationToken = default)
    {
        var fileInfo = await _fileInfoRepository.GetFileInfoAsync(fileGuid, cancellationToken);
        if (fileInfo == null)
        {
            return new NotFound("File info not found");
        }

        return fileInfo;
    }

    public async Task<List<FileInfo>> GetFileInfosAsync(List<Guid> fileGuids, CancellationToken cancellationToken = default)
    {
        var fileInfos = await _fileInfoRepository.GetFileInfosAsync(fileGuids, cancellationToken);
        return fileInfos;
    }

    public async Task<OneOf<FileData, NotFound>> GetReadStreamAsync(FileRequest fileRequest, CancellationToken cancellationToken = default)
    {
        var fileInfo = await _fileInfoRepository.GetFileInfoAsync(fileRequest.Guid, cancellationToken);
        if (fileInfo == null || !string.Equals(fileInfo.OriginalName + fileInfo.FileExtension, fileRequest.OriginalName))
        {
            return new NotFound("No file found with specified parameters");
        }
        
        var stream = await _fileStorage.GetReadStreamAsync(fileInfo, cancellationToken);
        if (stream == null)
        {
            return new NotFound("File not found");
        }

        var fileData = new FileData
        {
            FileInfoModel = fileInfo.ToFileInfoModel(),
            Stream = stream
        };
        return fileData;
    }

    public async Task<OneOf<FileInfo, InvalidInput>> CreateAsync(NewFile newFile, CancellationToken cancellationToken = default)
    {
        var error = new InvalidInput();
        if (string.IsNullOrWhiteSpace(newFile.MediaType))
        {
            error.Add("Media type (a.k.a. mime type) not set");
        }

        if (string.IsNullOrWhiteSpace(newFile.NameWithExtension))
        {
            error.Add("Original filename not set");
        }

        if (newFile.Stream == null || !newFile.Stream.CanRead || newFile.Stream.Length == 0)
        {
            error.Add("Input data stream cannot be read");
        }

        if (error.HasMessages)
        {
            return error;
        }

        var fileName = Path.GetFileNameWithoutExtension(newFile.NameWithExtension);
        var extension = newFile.NameWithExtension.Substring(fileName.Length);
        var fileInfo = new FileInfo
        {
            Guid = Guid.NewGuid(),
            OriginalName = fileName ?? Path.GetRandomFileName(),
            FileExtension = string.IsNullOrWhiteSpace(fileName) ? string.Empty : extension,
            Category = newFile.Category,
            MediaType = newFile.MediaType,
            Size = newFile.Stream!.Length,
            CreatedAt = DateTime.UtcNow
        };
        var dataWritten = await _fileStorage.WriteFileAsync(fileInfo, newFile.Stream!, cancellationToken);
        var fileInfoSaved = await _fileInfoRepository.AddFileInfoAsync(fileInfo, cancellationToken);
        return fileInfo;
    }
}