using System;
using System.Collections.Generic;

namespace DrugSchedule.StorageContract.Data;

public class TakingСonfirmation
{
    public long Id { get; set; }

    public required DateTime CreatedAt { get; set; }

    public List<FileInfo> Images { get; set; } = new();

    public string? Text { get; set; }
}