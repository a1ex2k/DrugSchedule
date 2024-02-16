using System;
using System.Collections.Generic;

namespace DrugSchedule.StorageContract.Data;

public class TakingСonfirmation
{
    public long Id { get; set; }

    public long RepeatId { get; set; }

    public required DateTime CreatedAt { get; set; }

    public DateOnly ForDate { get; set; }

    public DateOnly ForTime { get; set; }

    public TimeOfDay ForTimeOfDay { get; set; }

    public List<FileInfo> Images { get; set; } = new();

    public string? Text { get; set; }
}