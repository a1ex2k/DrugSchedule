using DrugSchedule.StorageContract.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DrugSchedule.Storage.Services;

public class FileStorageService : IFileStorage
{
    private readonly ILogger<FileStorageService> _logger;
    private IOptions<FileStorageOptions> _options;
    private readonly string _directoryPath;

    public FileStorageService(ILogger<FileStorageService> logger, IOptions<FileStorageOptions> options)
    {
        _logger = logger;
        _options = options;
        _directoryPath = options.Value.DirectoryPath;
    }

    public Task<Stream?> GetReadStreamAsync(Contract.FileInfo fileInfo, CancellationToken cancellationToken = default)
    {
        var fileName = $"{fileInfo.Guid}.{fileInfo.FileExtension}";
        var filePath = Path.Combine(_directoryPath, fileInfo.Category.ToString(), fileName);

        try
        {
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return Task.FromResult((Stream?)fileStream);
        }
        catch (FileNotFoundException ex)
        {
            _logger.LogWarning(ex, "File not found: Guid={Guid}, Ext={Ext}", fileInfo.Guid, fileInfo.FileExtension);
        }
        catch (DirectoryNotFoundException ex)
        {
            _logger.LogWarning(ex, "Directory not found for file: Guid={Guid}, Ext={Ext}, Category={Category}", 
                fileInfo.Guid, fileInfo.FileExtension, fileInfo.Category.ToString());
        }

        return Task.FromResult<Stream?>(null);
    }

    public async Task<bool> WriteFileAsync(Contract.FileInfo fileInfo, Stream stream, CancellationToken cancellationToken = default)
    {
        var fileName = $"{fileInfo.Guid}.{fileInfo.FileExtension}";
        var filePath = Path.Combine(_directoryPath, fileInfo.Category.ToString(), fileName);
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        var saved = false;
        try
        {
            await using var fileStream = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.None);
            await stream.CopyToAsync(fileStream, cancellationToken);
            saved = true;
        }
        catch (IOException ex)
        {
            _logger.LogWarning(ex, "File already exits: Guid={Guid}, Ext={Ext}", fileInfo.Guid, fileInfo.FileExtension);
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogInformation(ex, "Cancelled saving file: Guid={Guid}, Ext={Ext}", fileInfo.Guid, fileInfo.FileExtension);
            File.Delete(filePath);
        }

        return saved;
    }

    public Task<bool> RemoveFileAsync(Contract.FileInfo fileInfo, CancellationToken cancellationToken = default)
    {
        var filePath = Path.Combine(_directoryPath, fileInfo.Category.ToString(),
            fileInfo.Guid + fileInfo.FileExtension);
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

        return Task.FromResult(deleted);
    }
}