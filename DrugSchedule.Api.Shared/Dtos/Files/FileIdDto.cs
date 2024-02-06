using System;

namespace DrugSchedule.Api.Shared.Dtos;

public class FileIdDto
{
    public required Guid FileGuid { get; set; }
}