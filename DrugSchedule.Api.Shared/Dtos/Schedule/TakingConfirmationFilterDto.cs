using System;
using System.Collections.Generic;

namespace DrugSchedule.StorageContract.Data;

public class TakingConfirmationFilterDto
{
    public required List<long>? RepeatIds { get; set; }

    public required long ScheduleId { get; set; }

    public required DateOnly? ForDateMin { get; set; }

    public required DateOnly? ForDateMax { get; set; }
    
    public int Skip { get; set; }

    public int Take { get; set; }
}