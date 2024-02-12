using DrugSchedule.Api.FileAccessProvider;
using Microsoft.AspNetCore.Mvc;
using DrugSchedule.BusinessLogic.Services;
using DrugSchedule.BusinessLogic.Utils;
using DrugSchedule.BusinessLogic.Models;

namespace DrugSchedule.Api.Controllers;

[Route("api/[controller]/[action]")]
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
    [Route("{fileGuid}")]
    public async Task<IActionResult> DownloadPublic([FromRoute] Guid fileGuid, [FromQuery] bool thumb = false, CancellationToken cancellationToken = default)
    {
        var fileResult = thumb ?
            await _fileService.GetFileDataAsync(fileGuid, cancellationToken)
        : await _fileService.GetThumbnailAsync(fileGuid, cancellationToken);

        if (fileResult.IsT1)
        {
            return NotFound(fileResult.AsT1);
        }

        var fileData = fileResult.AsT0;

        if (!fileData.FileInfo.IsPublic())
        {
            return Unauthorized("A valid access key is required to download the file");
        }

        return CreateFileResult(fileData, thumb);
    }


    [HttpGet]
    [Route("{fileGuid}")]
    public async Task<IActionResult> DownloadPrivate([FromRoute] Guid fileGuid, [FromQuery] string accessKey,
        [FromQuery] int expiry, [FromQuery] string signature, [FromQuery] bool thumb = false, CancellationToken cancellationToken = default)
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

        var fileResult = thumb ?
            await _fileService.GetFileDataAsync(fileGuid, cancellationToken)
            : await _fileService.GetThumbnailAsync(fileGuid, cancellationToken);

        if (fileResult.IsT1)
        {
            return NotFound(fileResult.AsT1);
        }

        var fileData = fileResult.AsT0;
        return CreateFileResult(fileData, thumb);
    }

    private FileStreamResult CreateFileResult(FileData fileData, bool thumb)
    {
        var result = thumb ?
            new FileStreamResult(fileData.Stream, "image/jpeg")
            {
                FileDownloadName = $"{fileData.FileInfo.OriginalName}.thumb.jpg",
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