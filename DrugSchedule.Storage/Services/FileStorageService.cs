using DrugSchedule.StorageContract.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DrugSchedule.Storage.Services;

public class FileStorageService : IFileStorage
{
    private const string ThumbnailSuffix = "thumb";

    private readonly ILogger<FileStorageService> _logger;
    private IOptions<FileStorageOptions> _options;
    private readonly string _directoryPath;

    public FileStorageService(ILogger<FileStorageService> logger, IOptions<FileStorageOptions> options)
    {
        _logger = logger;
        _options = options;
        _directoryPath = options.Value.DirectoryPath;
    }

    public async Task<Stream?> GetReadStreamAsync(Contract.FileInfo fileInfo, CancellationToken cancellationToken = default)
    {
        var filePath = GetFilePath(fileInfo);
        var fileStream = GetReadStreamInternal(filePath);
        return fileStream;
    }

    public async Task<Stream?> GetThumbnailStreamAsync(Contract.FileInfo fileInfo, CancellationToken cancellationToken = default)
    {
        var filePath = GetThumbnailPath(fileInfo);
        if (!File.Exists(filePath))
        {
            return null;
        }
        var fileStream = GetReadStreamInternal(filePath);
        return fileStream;
    }

    public async Task<bool> WriteFileAsync(Contract.FileInfo fileInfo, Stream stream, CancellationToken cancellationToken = default)
    {
        var filePath = GetFilePath(fileInfo);
        var saved = await WriteFileInternalAsync(filePath, stream, cancellationToken);
        return saved;
    }

    public async Task<bool> WriteThumbnailAsync(Contract.FileInfo fileInfo, Stream stream, CancellationToken cancellationToken = default)
    {
        var filePath = GetThumbnailPath(fileInfo);
        var saved = await WriteFileInternalAsync(filePath, stream, cancellationToken);
        return saved;
    }

    public async Task<bool> RemoveFileAsync(Contract.FileInfo fileInfo, CancellationToken cancellationToken = default)
    {
        var filePath = GetFilePath(fileInfo);
        var deleted = false;
        try
        {
            File.Delete(filePath);
            deleted = true;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "File cannot be deleted: Guid={Guid}, Ext={Ext}", fileInfo.Guid, fileInfo.FileExtension);
        }

        return deleted;
    }


    private string GetFilePath(DrugSchedule.StorageContract.Data.FileInfo fileInfo)
    {
        return Path.Combine(_directoryPath, fileInfo.Category.ToString(),
            $"{fileInfo.Guid}.{fileInfo.FileExtension}");
    }

    private string GetThumbnailPath(DrugSchedule.StorageContract.Data.FileInfo fileInfo)
    {
        return Path.Combine(_directoryPath, fileInfo.Category.ToString(),
            $"{fileInfo.Guid}.{ThumbnailSuffix}.{fileInfo.FileExtension}");
    }

    private Stream? GetReadStreamInternal(string path)
    {
        FileStream? fileStream = null;
        try
        {
            fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        }
        catch (FileNotFoundException ex)
        {
            _logger.LogWarning(ex, "File not found: {Path}", path);
        }
        catch (DirectoryNotFoundException ex)
        {
            _logger.LogWarning(ex, "Directory not found for file: {Path}", path);
        }

        return fileStream;
    }

    private async Task<bool> WriteFileInternalAsync(string path, Stream stream, CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        var saved = false;
        try
        {
            await using var fileStream = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None);
            stream.Position = 0;
            await stream.CopyToAsync(fileStream, cancellationToken);
            saved = true;
        }
        catch (IOException ex)
        {
            _logger.LogWarning(ex, "File already exits: {Path}", path);
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogInformation(ex, "Cancelled saving file: {Path}", path);
            File.Delete(path);
        }

        return saved;
    }
}