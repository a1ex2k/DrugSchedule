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
    public async Task<IActionResult> GetSingle([FromBody] UserMedicamentIdDto dto, CancellationToken cancellationToken)
    {
        var medicamentResult = await _drugLibraryService.GetMedicamentSimpleAsync(dto.UserMedicamentId, cancellationToken);
        return medicamentResult.Match<IActionResult>(
            m => Ok(m.Adapt<UserMedicamentSimpleDto>()),
            error => NotFound(error.ToDto()));
    }


    [HttpPost]
    public async Task<IActionResult> GetSingleExtended([FromBody] UserMedicamentIdDto dto, CancellationToken cancellationToken)
    {
        var medicamentResult = await _drugLibraryService.GetMedicamentExtendedAsync(dto.UserMedicamentId, cancellationToken);
        return medicamentResult.Match<IActionResult>(
            m => Ok(m.Adapt<UserMedicamentExtendedDto>()),
            error => NotFound(error.ToDto()));
    }


    [HttpPost]
    public async Task<IActionResult> GetMany([FromBody] UserMedicamentFilterDto dto,
        CancellationToken cancellationToken)
    {
        var medicamentResult =
            await _drugLibraryService.GetMedicamentsSimpleAsync(dto.Adapt<UserMedicamentFilter>(), cancellationToken);
        return Ok(medicamentResult.Adapt<UserMedicamentSimpleCollectionDto>());
    }


    [HttpPost]
    public async Task<IActionResult> GetManyExtended([FromBody] UserMedicamentFilterDto dto, CancellationToken cancellationToken)
    {
        var medicamentResult =
            await _drugLibraryService.GetMedicamentsExtendedAsync(dto.Adapt<UserMedicamentFilter>(), cancellationToken);
        return Ok(medicamentResult.Adapt<UserMedicamentExtendedCollectionDto>());
    }


    [HttpPost]
    public async Task<IActionResult> GetSharedExtended([FromBody] UserMedicamentIdDto dto, CancellationToken cancellationToken)
    {
        var medicamentResult =
            await _drugLibraryService.GetSharedUserMedicamentAsync(dto.UserMedicamentId, cancellationToken);
        return medicamentResult.Match<IActionResult>(
            m => Ok(m.Adapt<UserMedicamentExtendedDto>()),
            error => NotFound(error.ToDto()));
    }


    [HttpPost]
    public async Task<IActionResult> Add([FromBody] NewUserMedicamentDto dto, CancellationToken cancellationToken)
    {
        var addResult =
            await _drugLibraryService.CreateMedicamentAsync(dto.Adapt<NewUserMedicament>(), cancellationToken);

        return addResult.Match<IActionResult>(
            id => Ok(new UserMedicamentIdDto {UserMedicamentId = id}),
            error => NotFound(error.ToDto()));
    }


    [HttpPost]
    public async Task<IActionResult> AddImage([FromForm] UserMedicamentIdDto userMedicamentId, [FromForm] IFormFile file, CancellationToken cancellationToken)
    {
        var inputFile = new InputFile
        {
            NameWithExtension = file.Name,
            MediaType = file.ContentType,
            Stream = file.OpenReadStream()
        };

        var addResult =
            await _drugLibraryService.AddImageAsync(userMedicamentId.UserMedicamentId, inputFile, cancellationToken);
        return addResult.Match<IActionResult>(
            f => Ok(f.Adapt<DownloadableFileDto>()),
            error => NotFound(error.ToDto()),
            invalid => BadRequest(invalid.ToDto()));
    }


    [HttpPost]
    public async Task<IActionResult> RemoveImage([FromBody] UserMedicamentImageRemoveDto dto, CancellationToken cancellationToken)
    {
        var removeResult =
            await _drugLibraryService.RemoveImageAsync(dto.UserMedicamentId, dto.FileGuid, cancellationToken);
        return removeResult.Match<IActionResult>(
            ok => Ok("Image removed"),
            error => NotFound(error.ToDto()));
    }


    [HttpPost]
    public async Task<IActionResult> Update(UserMedicamentUpdateDto dto, CancellationToken cancellationToken)
    {
        var updateResult = await _drugLibraryService.UpdateMedicamentAsync(dto.Adapt<UserMedicamentUpdate>(), cancellationToken);
        return updateResult.Match<IActionResult>(
            id => Ok(new UserMedicamentIdDto { UserMedicamentId = id }),
            notFound => NotFound(notFound.ToDto()),
            errorInput => BadRequest(errorInput.ToDto()));
    }


    [HttpPost]
    public async Task<IActionResult> Remove(UserMedicamentIdDto dto, CancellationToken cancellationToken)
    {
        var removeResult = await _drugLibraryService.RemoveMedicamentAsync(dto.UserMedicamentId, cancellationToken);
        return removeResult.Match<IActionResult>(
            ok => Ok("Medicament removed"),
            notFound => NotFound(notFound.ToDto()),
            invalid => BadRequest(invalid.ToDto()));
    }
}