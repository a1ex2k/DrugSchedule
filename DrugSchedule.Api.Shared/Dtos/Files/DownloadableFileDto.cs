using System;

namespace DrugSchedule.Api.Shared.Dtos;

public class DownloadableFileDto
{
    public required Guid Guid { get; set; }

    public required string DownloadUrl { get; set; }

    public required string? ThumbnailUrl { get; set; }
    
    public required string Name { get; set; }

    public required string MediaType { get; set; }

    public required long Size { get; set; }
}