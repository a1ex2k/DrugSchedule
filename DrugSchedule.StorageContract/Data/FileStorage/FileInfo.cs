using System;

namespace DrugSchedule.StorageContract.Data;

public class FileInfo
{
    public required Guid Guid { get; set; }

    public required string OriginalName { get; set; }

    public required string FileExtension { get; set; }

    public required FileCategory Category { get; set; }

    public required string MediaType { get; set; }

    public required long Size { get; set; }

    public required DateTime CreatedAt { get; set; }

    public required bool HasThumbnail { get; set; }
}