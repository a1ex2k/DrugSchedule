using DrugSchedule.Api.FileAccessProvider;
using Microsoft.AspNetCore.Mvc;
using DrugSchedule.Services.Services;
using DrugSchedule.Services.Utils;
using DrugSchedule.Services.Models;

namespace DrugSchedule.Api.Controllers;

[ApiController]
public class FilesController : ControllerBase
{
    private readonly IFileService _fileService;
    private readonly IFileAccessService _accessService;

    public FilesController(IFileService fileService, IFileAccessService accessService)
    {
        _fileService = fileService;
        _accessService = accessService;
    }


    [HttpGet]
    [Route("files/public/{fileGuid}")]
    public async Task<IActionResult> DownloadPublic([FromRoute] Guid fileGuid, CancellationToken cancellationToken = default)
    {
        return await BuildResult(fileGuid, false, cancellationToken);
    }


    [HttpGet]
    [Route("files/private/{fileGuid}")]
    public async Task<IActionResult> DownloadPrivate(
        [FromRoute] Guid fileGuid, [FromQuery] string accessKey,
        [FromQuery] int expiry, [FromQuery] string signature, 
        CancellationToken cancellationToken = default)
    {
        var accessParams = new FileAccessParams
        {
            FileGuid = fileGuid,
            AccessKey = accessKey,
            ExpiryTime = expiry,
            Signature = signature
        };
        var areValid = _accessService.Validate(accessParams);
        if (!areValid)
        {
            return Unauthorized("A valid access key is required to download the file");
        }

        return await BuildResult(fileGuid, false, cancellationToken);
    }

    [HttpGet]
    [Route("files/public/thumb/{fileGuid}")]
    public async Task<IActionResult> DownloadPublicThumbnail([FromRoute] Guid fileGuid, CancellationToken cancellationToken = default)
    {
        return await BuildResult(fileGuid, true, cancellationToken);
    }


    [HttpGet]
    [Route("files/private/thumb/{fileGuid}")]
    public async Task<IActionResult> DownloadPrivateThumbnail(
        [FromRoute] Guid fileGuid, [FromQuery] string accessKey,
        [FromQuery] int expiry, [FromQuery] string signature,
        CancellationToken cancellationToken = default)
    {
        var accessParams = new FileAccessParams
        {
            FileGuid = fileGuid,
            AccessKey = accessKey,
            ExpiryTime = expiry,
            Signature = signature
        };
        var areValid = _accessService.Validate(accessParams);
        if (!areValid)
        {
            return Unauthorized("A valid access key is required to download the thumbnail");
        }

        return await BuildResult(fileGuid, true, cancellationToken);
    }


    private async Task<IActionResult> BuildResult(Guid fileGuid, bool thumbnail = false, CancellationToken cancellationToken = default)
    {
        var fileResult = thumbnail ?
            await _fileService.GetFileDataAsync(fileGuid, cancellationToken)
            : await _fileService.GetThumbnailAsync(fileGuid, cancellationToken);

        if (fileResult.IsT1)
        {
            return NotFound(fileResult.AsT1);
        }

        var fileData = fileResult.AsT0;
        return CreateFileResult(fileData, thumbnail);
    }

    private FileStreamResult CreateFileResult(FileData fileData, bool thumbnail)
    {
        var result = thumbnail ?
            new FileStreamResult(fileData.Stream, "image/png")
            {
                FileDownloadName = $"{fileData.FileInfo.OriginalName}.thumb.png",
                LastModified = fileData.FileInfo.CreatedAt,
            }
            :
            new FileStreamResult(fileData.Stream, fileData.FileInfo.MediaType)
            {
                FileDownloadName = $"{fileData.FileInfo.OriginalName}.{fileData.FileInfo.FileExtension}",
                LastModified = fileData.FileInfo.CreatedAt,
            };
        return result;
    }
}