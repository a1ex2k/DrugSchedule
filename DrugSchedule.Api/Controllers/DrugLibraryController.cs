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
public class DrugLibraryController : ControllerBase
{
    private readonly IDrugLibraryService _drugLibraryService;

    public DrugLibraryController(IDrugLibraryService drugLibraryService)
    {
        _drugLibraryService = drugLibraryService;
    }


    [HttpPost]
    public async Task<IActionResult> GetMedicament([FromBody] MedicamentIdDto dto, CancellationToken cancellationToken)
    {
        var medicamentResult = await _drugLibraryService.GetMedicamentAsync(dto.MedicamentId, cancellationToken);
        return medicamentResult.Match<IActionResult>(
            m => Ok(m.Adapt<MedicamentSimpleDto>()),
            error => NotFound(error.ToDto()));
    }


    [HttpPost]
    public async Task<IActionResult> GetMedicamentExtended([FromBody] MedicamentIdDto dto, CancellationToken cancellationToken)
    {
        var medicamentResult = await _drugLibraryService.GetMedicamentExtendedAsync(dto.MedicamentId, cancellationToken);
        return medicamentResult.Match<IActionResult>(
            m => Ok(m.Adapt<MedicamentExtendedDto>()),
            error => NotFound(error.ToDto()));
    }


    [HttpPost]
    public async Task<IActionResult> GetMedicaments([FromBody] MedicamentFilterDto dto,
        CancellationToken cancellationToken)
    {
        var medicamentResult =
            await _drugLibraryService.GetMedicamentsAsync(dto.Adapt<MedicamentFilter>(), cancellationToken);
        return Ok(medicamentResult.Adapt<MedicamentSimpleCollectionDto>());
    }


    [HttpPost]
    public async Task<IActionResult> GetMedicamentsExtended([FromBody] MedicamentFilterDto dto, CancellationToken cancellationToken)
    {
        var medicamentResult =
            await _drugLibraryService.GetMedicamentsExtendedAsync(dto.Adapt<MedicamentFilter>(), cancellationToken);
        return Ok(medicamentResult.Adapt<MedicamentExtendedCollectionDto>());
    }


    [HttpPost]
    public async Task<IActionResult> GetManufacturer([FromBody] ManufacturerIdDto dto, CancellationToken cancellationToken)
    {
        var manufacturerResult =
            await _drugLibraryService.GetManufacturerAsync(dto.ManufacturerId, cancellationToken);
        return manufacturerResult.Match<IActionResult>(
            m => Ok(m.Adapt<ManufacturerDto>()),
            error => NotFound(error.ToDto()));
    }


    [HttpPost]
    public async Task<IActionResult> GetManufacturers([FromBody] ManufacturerFilterDto dto, CancellationToken cancellationToken)
    {
        var manufacturerResult =
            await _drugLibraryService.GetManufacturersAsync(dto.Adapt<ManufacturerFilter>(), cancellationToken);
        return Ok(manufacturerResult.Adapt<ManufacturerCollectionDto>());
    }


    [HttpPost]
    public async Task<IActionResult> GetReleaseForms([FromBody] MedicamentReleaseFormFilterDto dto, CancellationToken cancellationToken)
    {
        var formResult =
            await _drugLibraryService.GetReleaseFormsAsync(dto.Adapt<MedicamentReleaseFormFilter>(), cancellationToken);
        return Ok(formResult.Adapt<ReleaseFormCollectionDto>());
    }
}