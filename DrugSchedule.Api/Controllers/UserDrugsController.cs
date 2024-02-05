using DrugSchedule.Api.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using DrugSchedule.BusinessLogic.Models;
using DrugSchedule.BusinessLogic.Services;
using DrugSchedule.BusinessLogic.Services.Abstractions;
using DrugSchedule.StorageContract.Data;
using Mapster;
using Microsoft.AspNetCore.Authorization;

namespace DrugSchedule.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class UserDrugsController : ControllerBase
{
    private readonly IUserDrugLibrary _drugLibraryService;

    public UserDrugsController(IUserDrugLibrary drugLibraryService)
    {
        _drugLibraryService = drugLibraryService;
    }


    [HttpPost]
    public async Task<IActionResult> GetMedicament([FromBody] UserMedicamentIdDto dto, CancellationToken cancellationToken)
    {
        var medicamentResult = await _drugLibraryService.GetMedicamentSimpleAsync(dto.UserMedicamentId, cancellationToken);
        return medicamentResult.Match(
            m => (IActionResult)Ok(medicamentResult.Adapt<UserMedicamentSimpleDto>()),
            error => (IActionResult)NotFound(error.Message));
    }


    [HttpPost]
    public async Task<IActionResult> GetMedicamentExtended([FromBody] UserMedicamentIdDto dto, CancellationToken cancellationToken)
    {
        var medicamentResult = await _drugLibraryService.GetMedicamentExtendedAsync(dto.UserMedicamentId, cancellationToken);
        return medicamentResult.Match(
            m => (IActionResult)Ok(medicamentResult.Adapt<UserMedicamentExtendedDto>()),
            error => (IActionResult)NotFound(error.Message));
    }


    [HttpPost]
    public async Task<IActionResult> GetMedicaments([FromBody] UserMedicamentFilterDto dto,
        CancellationToken cancellationToken)
    {
        var medicamentResult =
            await _drugLibraryService.GetMedicamentsSimpleAsync(dto.Adapt<UserMedicamentFilter>(), cancellationToken);
        return Ok(medicamentResult.Adapt<UserMedicamentSimpleCollectionDto>());
    }


    [HttpPost]
    public async Task<IActionResult> GetMedicamentsExtended([FromBody] UserMedicamentFilterDto dto, CancellationToken cancellationToken)
    {
        var medicamentResult =
            await _drugLibraryService.GetMedicamentsExtendedAsync(dto.Adapt<UserMedicamentFilter>(), cancellationToken);
        return Ok(medicamentResult.Adapt<UserMedicamentSimpleCollectionDto>());
    }


    [HttpPost]
    public async Task<IActionResult> AddMedicamentImage([FromRoute] long medicamentId, [FromBody] IFormFile formFile, CancellationToken cancellationToken)
    {
        var inputFile = new InputFile
        {
            NameWithExtension = formFile.Name,
            MediaType = formFile.ContentType,
            Stream = formFile.OpenReadStream()
        };

        var addResult =
            await _drugLibraryService.AddImageAsync(medicamentId, inputFile, cancellationToken);
        return addResult.Match(
            f => (IActionResult)Ok(f.Adapt<DownloadableFileDto>()),
            error => (IActionResult)NotFound(error.Message),
            invalid => BadRequest(invalid.ErrorsList));
    }


    [HttpPost]
    public async Task<IActionResult> RemoveMedicamentImage([FromBody] UserMedicamentImageRemoveDto dto, CancellationToken cancellationToken)
    {
        var removeResult =
            await _drugLibraryService.RemoveImageAsync(dto.MedicamentId, dto.ImageId.Adapt<FileId>(), cancellationToken);
        return removeResult.Match(
            ok => (IActionResult)Ok(),
            error => (IActionResult)NotFound(error.Message));
    }
}