using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DrugSchedule.Services.Services;

namespace DrugSchedule.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public partial class ScheduleController : ControllerBase
{
    private readonly IScheduleService _scheduleService;
    private readonly IScheduleManipulatingService _scheduleManipulating;
    private readonly IScheduleConfirmationManipulatingService _confirmationManipulating;

    public ScheduleController(IScheduleConfirmationManipulatingService confirmationManipulating, IScheduleManipulatingService scheduleManipulating, IScheduleService scheduleService)
    {
        _confirmationManipulating = confirmationManipulating;
        _scheduleManipulating = scheduleManipulating;
        _scheduleService = scheduleService;
    }
}