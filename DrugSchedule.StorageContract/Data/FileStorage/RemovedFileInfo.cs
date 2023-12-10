using System;

namespace DrugSchedule.StorageContract.Data;

public class RemovedFileInfo
{
    public required Guid Guid { get; set; }

    public required string FileName { get; set; }

    public required DateTime RemovedAt { get; set; }
}