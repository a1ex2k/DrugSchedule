using System;
using System.Collections.Generic;

namespace DrugSchedule.StorageContract.Data;

public class TakingConfirmationFilter : FilterBase
{
    public required List<long>? RepeatIds { get; set; }

    public required long ScheduleId { get; set; }

}