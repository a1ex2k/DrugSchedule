using System;

namespace DrugSchedule.StorageContract.Data;

public class FileInfo
{
    public required Guid Guid { get; set; }

    public required string FileName { get; set; }
    
    public required string ContentType { get; set; }

    public required long Size { get; set; }

    public required DateTime CreatedAt { get; set; }
}