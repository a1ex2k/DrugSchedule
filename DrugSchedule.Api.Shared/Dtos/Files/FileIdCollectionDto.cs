using System;
using System.Collections.Generic;

namespace DrugSchedule.Api.Shared.Dtos;

public class FileIdCollectionDto
{
    public required List<Guid> FilesGuids { get; set; }
}