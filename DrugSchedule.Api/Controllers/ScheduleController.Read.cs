using DrugSchedule.Api.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using DrugSchedule.StorageContract.Data;
using Mapster;
using DrugSchedule.Api.Utils;


namespace DrugSchedule.Api.Controllers;

public partial class ScheduleController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> SearchForSchedule([FromBody] ScheduleSearchDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _scheduleService.SearchForScheduleAsync(dto.Adapt<ScheduleSearch>(), cancellationToken);
        return result.Match<IActionResult>(
            scheduleCollection => Ok(scheduleCollection.Adapt<ScheduleSimpleCollectionDto>()),
            invalidInput => BadRequest(invalidInput.ToDto())
        );
    }

    [HttpPost]
    public async Task<IActionResult> GetScheduleSimple(ScheduleIdDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _scheduleService.GetScheduleSimpleAsync(dto.ScheduleId, cancellationToken);
        return result.Match<IActionResult>(
            schedule => Ok(schedule.Adapt<ScheduleSimpleDto>()),
            notFound => NotFound(notFound.ToDto())
        );
    }

    [HttpPost]
    public async Task<IActionResult> GetSchedulesSimple([FromBody] TakingScheduleFilterDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _scheduleService.GetSchedulesSimpleAsync(dto.Adapt<TakingScheduleFilter>(), cancellationToken);
        return Ok(result.Adapt<ScheduleSimpleCollectionDto>());
    }

    [HttpPost]
    public async Task<IActionResult> GetScheduleExtended(ScheduleIdDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _scheduleService.GetScheduleExtendedAsync(dto.ScheduleId, cancellationToken);
        return result.Match<IActionResult>(
            schedule => Ok(schedule.Adapt<ScheduleExtendedDto>()),
            notFound => NotFound(notFound.ToDto())
        );
    }

    [HttpPost]
    public async Task<IActionResult> GetSchedulesExtended([FromBody] TakingScheduleFilterDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _scheduleService.GetSchedulesExtendedAsync(dto.Adapt<TakingScheduleFilter>(), cancellationToken);
        return Ok(result.Adapt<ScheduleExtendedCollectionDto>());
    }

    [HttpPost]
    public async Task<IActionResult> GetTakingConfirmations([FromBody] TakingConfirmationFilterDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _scheduleService.GetTakingConfirmationsAsync(dto.Adapt<TakingConfirmationFilter>(), cancellationToken);
        return result.Match<IActionResult>(
            confirmationCollection => Ok(confirmationCollection.Adapt<TakingСonfirmationCollectionDto>()),
            notFound => NotFound(notFound.ToDto())
        );
    }


    [HttpPost]
    public async Task<IActionResult> GetTimetable([FromBody] TimetableFilterDto dto, CancellationToken cancellationToken = default)
    {
        if(dto.ScheduleId == null)
        {
            var result = await _scheduleService.GetTimetableAsync(dto.MinDate, dto.MaxDate, cancellationToken);
            return result.Match<IActionResult>(
                timetable => Ok(timetable.Adapt<TimetableDto>()),
                notFound => NotFound(notFound.ToDto()));
        }

        var resultOfSchedule = await _scheduleService.GetScheduleTimetableAsync(dto.ScheduleId.Value, dto.MinDate, dto.MaxDate, cancellationToken);
        return resultOfSchedule.Match<IActionResult>(
            timetable => Ok(timetable.Adapt<TimetableDto>()),
            notFound => NotFound(notFound.ToDto()),
            bad => BadRequest(bad.ToDto()));
    }
}