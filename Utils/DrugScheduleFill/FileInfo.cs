using System;

namespace DrugScheduleFill;

public class FileInfo
{
    public required Guid Guid { get; set; }

    public required string OriginalName { get; set; }

    public required string Extension { get; set; }

    public required FileCategory FileCategory { get; set; }

    public required string MediaType { get; set; }

    public required long Size { get; set; }

    public required DateTime CreatedAt { get; set; }
}