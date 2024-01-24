using System;
using System.Collections.Generic;

namespace DrugSchedule.Api.Shared.Dtos;

public class FileInfoRequestDto
{
    public required List<Guid> FilesGuid { get; set; }
}