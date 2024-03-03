using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Api.Utils;
using Microsoft.AspNetCore.Mvc;
using DrugSchedule.Services.Models;
using Mapster;

namespace DrugSchedule.Api.Controllers;

public partial class ScheduleController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateSchedule([FromBody] NewScheduleDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _scheduleManipulating.CreateScheduleAsync(dto.Adapt<NewSchedule>(), cancellationToken);
        return result.Match<IActionResult>(
            scheduleId => Ok(new ScheduleIdDto{ ScheduleId = scheduleId }),
            notFound => NotFound(notFound.ToDto()),
            invalidInput => BadRequest(invalidInput.ToDto())
        );
    }

    [HttpPost]
    public async Task<IActionResult> UpdateSchedule([FromBody] ScheduleUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _scheduleManipulating.UpdateScheduleAsync(dto.Adapt<ScheduleUpdate>(), cancellationToken);
        return result.Match<IActionResult>(
            scheduleId => Ok(new ScheduleIdDto { ScheduleId = scheduleId }),
            notFound => NotFound(notFound.ToDto()),
            invalidInput => BadRequest(invalidInput.ToDto())
        );
    }

    [HttpPost]
    public async Task<IActionResult> RemoveSchedule([FromBody] ScheduleIdDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _scheduleManipulating.RemoveSchedule(dto.ScheduleId, cancellationToken);
        return result.Match<IActionResult>(
            success => Ok("Schedule removed"),
            notFound => NotFound(notFound.ToDto())
        );
    }

    [HttpPost]
    public async Task<IActionResult> CreateRepeat([FromBody] NewScheduleRepeatDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _scheduleManipulating.CreateRepeatAsync(dto.Adapt<NewScheduleRepeat>(), cancellationToken);
        return result.Match<IActionResult>(
            repeatId => Ok(new RepeatIdDto { RepeatId = repeatId }),
            notFound => NotFound(notFound.ToDto()),
            invalidInput => BadRequest(invalidInput.ToDto())
        );
    }

    [HttpPost]
    public async Task<IActionResult> UpdateRepeat([FromBody] ScheduleRepeatUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _scheduleManipulating.UpdateRepeatAsync(dto.Adapt<ScheduleRepeatUpdate>(), cancellationToken);
        return result.Match<IActionResult>(
            repeatId => Ok(new RepeatIdDto { RepeatId = repeatId }),
            notFound => NotFound(notFound.ToDto()),
            invalidInput => BadRequest(invalidInput.ToDto())
        );
    }

    [HttpPost]
    public async Task<IActionResult> RemoveRepeat([FromBody] RepeatIdDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _scheduleManipulating.RemoveRepeatAsync(dto.RepeatId, cancellationToken);
        return result.Match<IActionResult>(
            success => Ok("Repeat removed"),
            notFound => NotFound(notFound.ToDto())
        );
    }

    [HttpPost]
    public async Task<IActionResult> AddOrUpdateShare([FromBody] ScheduleShareUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _scheduleManipulating.AddOrUpdateShareAsync(dto.Adapt<ScheduleShareUpdate>(), cancellationToken);
        return result.Match<IActionResult>(
            success => Ok("Schedule shared"),
            notFound => NotFound(notFound.ToDto()),
            invalidInput => BadRequest(invalidInput.ToDto())
        );
    }

    [HttpPost]
    public async Task<IActionResult> RemoveShare([FromBody] ScheduleShareRemoveDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _scheduleManipulating.RemoveShareAsync(dto.ScheduleId, dto.CommonContactProfileId, cancellationToken);
        return result.Match<IActionResult>(
            success => Ok(),
            notFound => NotFound(notFound.ToDto())
        );
    }

    [HttpPost]
    public async Task<IActionResult> CreateConfirmation([FromBody] NewTakingСonfirmationDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _confirmationManipulating.CreateConfirmationAsync(dto.Adapt<NewTakingСonfirmation>(), cancellationToken);
        return result.Match<IActionResult>(
            confirmationId => Ok(confirmationId),
            notFound => NotFound(notFound.ToDto()),
            invalidInput => BadRequest(invalidInput.ToDto())
        );
    }

    [HttpPost]
    public async Task<IActionResult> UpdateConfirmation([FromBody] TakingСonfirmationUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _confirmationManipulating.UpdateConfirmationAsync(dto.Adapt<TakingСonfirmationUpdate>(), cancellationToken);
        return result.Match<IActionResult>(
            confirmationId => Ok(confirmationId.Adapt<ConfirmationIdDto>()),
            notFound => NotFound(notFound.ToDto()),
            invalidInput => BadRequest(invalidInput.ToDto())
        );
    }

    [HttpPost]
    public async Task<IActionResult> RemoveConfirmation([FromBody] ConfirmationIdDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _confirmationManipulating.RemoveConfirmationAsync(new (dto.ConfirmationId, dto.RepeatId), cancellationToken);
        return result.Match<IActionResult>(
            success => Ok("Confirmation removed"),
            notFound => NotFound(notFound.ToDto())
        );
    }

    [HttpPost]
    public async Task<IActionResult> AddConfirmationImage([FromForm] ConfirmationId confirmationId, [FromForm] IFormFile file, CancellationToken cancellationToken = default)
    {
        var inputFile = new InputFile
        {
            NameWithExtension = file.Name,
            MediaType = file.ContentType,
            Stream = file.OpenReadStream()
        };

        var result = await _confirmationManipulating.AddConfirmationImageAsync(confirmationId.Adapt<ConfirmationId>(), inputFile, cancellationToken);
        return result.Match<IActionResult>(
            file => Ok(file.Adapt<DownloadableFileDto>()),
            notFound => NotFound(notFound.ToDto()),
            invalidInput => BadRequest(invalidInput.ToDto())
        );
    }

    [HttpPost]
    public async Task<IActionResult> RemoveConfirmationImage([FromBody] ConfirmationImageRemoveDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _confirmationManipulating.RemoveConfirmationImageAsync(new(dto.ConfirmationId, dto.RepeatId), dto.FileGuid, cancellationToken);
        return result.Match<IActionResult>(
            success => Ok("Image removed"),
            notFound => NotFound(notFound.ToDto())
        );
    }
}
