using System;
using System.Collections.Generic;

namespace DrugSchedule.StorageContract.Data.Schedule.Tool;

public record ConfirmationsRequestParameters
{
    public required List<(long RepeatId, DateOnly[] CaculatedDates)> RepeatIds { get; set; }
}