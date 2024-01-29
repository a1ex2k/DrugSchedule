using DrugSchedule.Api.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using DrugSchedule.BusinessLogic.Models;
using DrugSchedule.BusinessLogic.Services.Abstractions;
using DrugSchedule.StorageContract.Data;
using Mapster;

namespace DrugSchedule.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
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
        var medicamentResult = await _drugLibraryService.GetMedicamentAsync(dto.Id, cancellationToken);
        return medicamentResult.Match(
            m => (IActionResult)Ok(medicamentResult.Adapt<MedicamentSimpleDto>()),
            error => (IActionResult)NotFound(error.Message));
    }


    [HttpPost]
    public async Task<IActionResult> GetMedicamentExtended([FromBody] MedicamentIdDto dto, CancellationToken cancellationToken)
    {
        var medicamentResult = await _drugLibraryService.GetMedicamentExtendedAsync(dto.Id, cancellationToken);
        return medicamentResult.Match(
            m => (IActionResult)Ok(medicamentResult.Adapt<MedicamentExtendedDto>()),
            error => (IActionResult)NotFound(error.Message));
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
        return Ok(medicamentResult.Adapt<MedicamentSimpleCollectionDto>());
    }


    [HttpPost]
    public async Task<IActionResult> GetManufacturer([FromBody] ManufacturerIdDto dto, CancellationToken cancellationToken)
    {
        var manufacturerResult =
            await _drugLibraryService.GetManufacturerAsync(dto.Id, cancellationToken);
        return manufacturerResult.Match(
            m => (IActionResult)Ok(m.Adapt<ManufacturerDto>()),
            error => (IActionResult)NotFound(error.Message));
    }


    [HttpPost]
    public async Task<IActionResult> GetManufacturers([FromBody] ManufacturerFilterDto dto, CancellationToken cancellationToken)
    {
        var manufacturerResult =
            await _drugLibraryService.GetMedicamentsExtendedAsync(dto.Adapt<MedicamentFilter>(), cancellationToken);
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