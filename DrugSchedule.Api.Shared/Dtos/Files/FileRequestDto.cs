using System;

namespace DrugSchedule.Api.Shared.Dtos;

public class FileRequestDto
{
    public required Guid Guid { get; set; }

    public required string OriginalName { get; set; }
}