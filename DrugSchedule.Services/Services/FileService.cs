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
    private readonly IFileProcessor _fileProcessor;
    private readonly IThumbnailService _thumbnailService;


    public FileService(IFileInfoRepository fileInfoRepository, IFileStorage fileStorage, IFileProcessor fileProcessor, IThumbnailService thumbnailService)
    {
        _fileInfoRepository = fileInfoRepository;
        _fileStorage = fileStorage;
        _fileProcessor = fileProcessor;
        _thumbnailService = thumbnailService;
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


    public async Task<OneOf<FileData, NotFound>> GetFileDataAsync(Guid fileGuid, CancellationToken cancellationToken = default)
    {
        if (fileGuid == Guid.Empty)
        {
            return new NotFound("Empty guid");
        }

        var fileInfo = await _fileInfoRepository.GetFileInfoAsync(fileGuid, cancellationToken);
        if (fileInfo == null)
        {
            return new NotFound("File not found");
        }

        var stream = await _fileStorage.GetReadStreamAsync(fileInfo, cancellationToken);
        if (stream == null)
        {
            return new NotFound("File not found");
        }

        var fileData = new FileData
        {
            FileInfo = fileInfo,
            Stream = stream
        };
        return fileData;
    }

    public async Task<OneOf<FileInfo, InvalidInput>> CreateAsync(InputFile inputFile, AwaitableFileParams awaitableFileParams, FileCategory category, CancellationToken cancellationToken = default)
    {
        var checkErrors = _fileProcessor.GetInputFileErrors(inputFile, awaitableFileParams);
        if (checkErrors != null)
        {
            return checkErrors;
        }

        var fileName = Path.GetFileNameWithoutExtension(inputFile.NameWithExtension);
        var extension = inputFile.NameWithExtension.Substring(fileName.Length + 1);
        var fileInfo = new FileInfo
        {
            Guid = Guid.NewGuid(),
            OriginalName = fileName ?? Path.GetRandomFileName(),
            FileExtension = string.IsNullOrWhiteSpace(fileName) ? string.Empty : extension,
            Category = category,
            MediaType = inputFile.MediaType,
            Size = inputFile.Stream.Length,
            CreatedAt = DateTime.UtcNow,
            HasThumbnail = false
        };
        cancellationToken.ThrowIfCancellationRequested();
        var dataWritten = await _fileStorage.WriteFileAsync(fileInfo, inputFile.Stream!, cancellationToken);
        var thumbWritten = await TryCreateThumbnailAsync(fileInfo, inputFile.Stream, awaitableFileParams, cancellationToken);
        fileInfo.HasThumbnail = thumbWritten;
        var fileInfoSaved = await _fileInfoRepository.AddFileInfoAsync(fileInfo, cancellationToken);
        return fileInfoSaved!;
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

    private async Task<bool> TryCreateThumbnailAsync(FileInfo fileInfo, Stream stream, AwaitableFileParams awaitableFileParams, CancellationToken cancellation = default)
    {
        if (!awaitableFileParams.TryCreateThumbnail)
        {
            return false;
        }

        try
        {
            using var thumbStream = await _thumbnailService.CreateJpgThumbnail(stream, fileInfo.MediaType, awaitableFileParams.CropThumbnail, cancellation);
            if (thumbStream != null)
            {
                var thumbWritten = await _fileStorage.WriteThumbnailAsync(fileInfo, thumbStream, cancellation);
                return thumbWritten;
            }
        }
        catch
        {
            return false;
        }
        return true;
    }
}