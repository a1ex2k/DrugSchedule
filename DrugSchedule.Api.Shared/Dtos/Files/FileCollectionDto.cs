using System.Collections.Generic;

namespace DrugSchedule.Api.Shared.Dtos;

public class FileCollectionDto
{
    public required List<DownloadableFileDto> Files { get; set; }
}