using System;

namespace DrugSchedule.Api.Shared.Dtos;

public class FileRequestDto
{
    public required Guid FileGuid { get; set; }

    public required long DownloadId { get; set; }
}