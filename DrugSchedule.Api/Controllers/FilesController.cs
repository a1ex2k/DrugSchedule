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
    public async Task<IActionResult> Download([FromRoute] Guid fileGuid, CancellationToken cancellationToken)
    {
        var fileResult = await _fileService.GetFileDataAsync(fileGuid, CancellationToken.None);
        if (fileResult.IsT1)
        {
            return NotFound(fileResult.AsT1);
        }

        var fileData = fileResult.AsT0;

        if (!fileData.FileInfo.IsPublic())
        {
            return Unauthorized("A valid access key is required to download the file");
        }

        return CreateFileResult(fileData);
    }

    [HttpGet]
    public async Task<IActionResult> Download([FromRoute] Guid fileGuid, [FromQuery] string accessKey,
        [FromQuery] int expiry, [FromQuery] string signature, CancellationToken cancellationToken)
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

        var fileResult = await _fileService.GetFileDataAsync(fileGuid, CancellationToken.None);
        if (fileResult.IsT1)
        {
            return NotFound(fileResult.AsT1);
        }

        var fileData = fileResult.AsT0;
        return CreateFileResult(fileData);
    }

    private FileStreamResult CreateFileResult(FileData fileData)
    {
        var result = new FileStreamResult(fileData.Stream, fileData.FileInfo.MediaType)
        {
            FileDownloadName = fileData.FileInfo.OriginalName + fileData.FileInfo.FileExtension
        };
        return result;
    }
}