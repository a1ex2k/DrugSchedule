using DrugSchedule.Api.ServerOnlyDtos;
using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Api.Utils;
using Microsoft.AspNetCore.Mvc;
using DrugSchedule.Services.Models;
using DrugSchedule.Services.Services.Abstractions;
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
        return medicamentResult.Match<IActionResult>(
            m => Ok(medicamentResult.Adapt<UserMedicamentSimpleDto>()),
            error => NotFound(error.ToDto()));
    }


    [HttpPost]
    public async Task<IActionResult> GetMedicamentExtended([FromBody] UserMedicamentIdDto dto, CancellationToken cancellationToken)
    {
        var medicamentResult = await _drugLibraryService.GetMedicamentExtendedAsync(dto.UserMedicamentId, cancellationToken);
        return medicamentResult.Match<IActionResult>(
            m => Ok(medicamentResult.Adapt<UserMedicamentExtendedDto>()),
            error => NotFound(error.ToDto()));
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
    public async Task<IActionResult> AddMedicamentImage([FromForm] NewUserMedicamentImageDto dto, CancellationToken cancellationToken)
    {
        var inputFile = new InputFile
        {
            NameWithExtension = dto.FormFile.Name,
            MediaType = dto.FormFile.ContentType,
            Stream = dto.FormFile.OpenReadStream()
        };

        var addResult =
            await _drugLibraryService.AddImageAsync(dto.UserMedicamentId, inputFile, cancellationToken);
        return addResult.Match<IActionResult>(
            f => Ok(f.Adapt<DownloadableFileDto>()),
            error => NotFound(error.ToDto()),
            invalid => BadRequest(invalid.ToDto()));
    }


    [HttpPost]
    public async Task<IActionResult> RemoveMedicamentImage([FromBody] UserMedicamentImageRemoveDto dto, CancellationToken cancellationToken)
    {
        var removeResult =
            await _drugLibraryService.RemoveImageAsync(dto.UserMedicamentId, dto.FileGuid, cancellationToken);
        return removeResult.Match<IActionResult>(
            ok => Ok("Image removed successfully"),
            error => NotFound(error.ToDto()));
    }
}