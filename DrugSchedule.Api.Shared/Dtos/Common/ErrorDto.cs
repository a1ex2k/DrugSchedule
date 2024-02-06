using System.Collections.Generic;

namespace DrugSchedule.Api.Shared.Dtos;

public class ErrorDto
{
    public required string Category { get; set; }

    public List<string> Messages { get; set; } = new List<string>();
}