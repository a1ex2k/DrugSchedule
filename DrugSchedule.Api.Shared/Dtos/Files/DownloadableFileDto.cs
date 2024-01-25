using System;

namespace DrugSchedule.Api.Shared.Dtos;

public class DownloadableFileDto
{
    public required Guid Guid { get; set; }

    public long DownloadId { get; set; }
    
    public required string NameWithExtension { get; set; }

    public required string MediaType { get; set; }

    public required long Size { get; set; }
}