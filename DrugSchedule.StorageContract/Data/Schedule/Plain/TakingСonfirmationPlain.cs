using System;
using System.Collections.Generic;

namespace DrugSchedule.StorageContract.Data;

public class TakingСonfirmationPlain
{
    public long Id { get; set; }

    public required DateTime CreatedAt { get; set; }

    public List<Guid> ImagesGuids { get; set; } = new();

    public string? Text { get; set; }

    public long ScheduleRepeatId { get; set; }
}