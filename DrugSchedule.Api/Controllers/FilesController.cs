using DrugSchedule.Api.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using DrugSchedule.BusinessLogic.Models;
using DrugSchedule.BusinessLogic.Services;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using System.IO;

namespace DrugSchedule.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class FilesController : ControllerBase
{
    private readonly IFileStore _fileStore;

    public FilesController(IFileStore fileStore, IFileInfoService fileInfoService)
    {
        _fileStore = fileStore;
    }


    [HttpPost]
    public async Task<IActionResult> Download([FromBody] FileRequestDto dto, CancellationToken cancellationToken)
    {
        var fileResult = await _fileStore.GetReadStreamAsync(dto.Adapt<FileRequest>(), CancellationToken.None);
        if (fileResult.IsT1)
        {
            return NotFound(fileResult.AsT1);
        }

        var fileData = fileResult.AsT0;
        var result = new FileStreamResult(fileData.Stream, fileData.FileInfoModel.MediaType);
        result.FileDownloadName = fileData.FileInfoModel.NameWithExtension;
        return Ok(result);
    }
}