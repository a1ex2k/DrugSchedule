using DrugSchedule.Api.FileAccessProvider;
using Microsoft.AspNetCore.Mvc;
using DrugSchedule.Services.Services;
using DrugSchedule.Services.Utils;
using DrugSchedule.Services.Models;
using OneOf.Types;

namespace DrugSchedule.Api.Controllers;

[ApiController]
public class FilesController : ControllerBase
{
    private const string UnAuthorizedError = "A valid access key is required to download the file";
    private readonly IFileService _fileService;
    private readonly IFileAccessService _accessService;

    public FilesController(IFileService fileService, IFileAccessService accessService)
    {
        _fileService = fileService;
        _accessService = accessService;
    }

    [HttpGet]
    [Route("files/{fileGuid}")]
    public async Task<IActionResult> Download(
        [FromRoute] Guid fileGuid, [FromQuery] bool thumb = false, [FromQuery] string accessKey = "",
        [FromQuery] int expiry = 0, [FromQuery] string signature = "", 
        CancellationToken cancellationToken = default)
    {
        var fileResult = !thumb ?
            await _fileService.GetFileDataAsync(fileGuid, cancellationToken)
            : await _fileService.GetThumbnailAsync(fileGuid, cancellationToken);

        if (fileResult.IsT1)
        {
            return NotFound(fileResult.AsT1);
        }

        var fileData = fileResult.AsT0;
        if (fileData.FileInfo.Category.IsPublic())
        {
            return CreateFileResult(fileData, thumb);
        }

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
            return Unauthorized(UnAuthorizedError);
        }

        return CreateFileResult(fileData, thumb);
    }

    private FileStreamResult CreateFileResult(FileData fileData, bool thumbnail)
    {
        var result = thumbnail ?
            new FileStreamResult(fileData.Stream, "image/png")
            {
                FileDownloadName = $"{fileData.FileInfo.OriginalName}_thumb.png",
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