using Microsoft.AspNetCore.Mvc;
using DrugSchedule.BusinessLogic.Services;
using DrugSchedule.BusinessLogic.Utils;

namespace DrugSchedule.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class FilesController : ControllerBase
{
    private readonly IFileService _fileService;
    private readonly IFileAccessKeyService _accessKeyService;

    public FilesController(IFileService fileService, IFileAccessKeyService accessKeyService)
    {
        _fileService = fileService;
        _accessKeyService = accessKeyService;
    }


    [HttpGet]
    public async Task<IActionResult> Download([FromRoute] Guid fileGuid, [FromQuery] string? accessKey, CancellationToken cancellationToken)
    {
        var fileResult = await _fileService.GetFileDataAsync(fileGuid, CancellationToken.None);
        if (fileResult.IsT1)
        {
            return NotFound(fileResult.AsT1);
        }

        var fileData = fileResult.AsT0;
        if (!fileData.FileInfo.IsPublic() && !_accessKeyService.ValidateKey(fileGuid, accessKey))
        {
            return Unauthorized("A valid access key is required to download the file");
        }

        var result = new FileStreamResult(fileData.Stream, fileData.FileInfo.MediaType)
        {
            FileDownloadName = fileData.FileInfo.OriginalName + fileData.FileInfo.FileExtension
        };
        return Ok(result);
    }
}

public interface IFileAccessKeyService
{
    bool ValidateKey(Guid fileGuid, string? accessKey);

    string GenerateKey(Guid fileGuid);
}

public class FileAccessKeyService : IFileAccessKeyService
{
    private string GenerateKey(Guid fileGuid)
    {
        


        fileGuid.

        return $"{fileGuid}:{DateTime.}"
    }

    public bool ValidateKey(Guid fileGuid, string? accessKey)
    {
        if (string.IsNullOrWhiteSpace(accessKey))
        {
            return false;
        }


    }

    public string GenerateKey(Guid fileGuid)
    {
        throw new NotImplementedException();
    }
}