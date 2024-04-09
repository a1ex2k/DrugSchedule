using System;
using System.ComponentModel.DataAnnotations;

namespace DrugScheduleFill.Models;

public class FileInfo
{
    [Key]
    public required Guid Guid { get; set; }

    public required string OriginalName { get; set; }

    public required string Extension { get; set; }

    public required FileCategory FileCategory { get; set; }

    public required string MediaType { get; set; }

    public required long Size { get; set; }

    public required DateTime CreatedAt { get; set; }

    public bool HasThumbnail { get; set; } = false;
}