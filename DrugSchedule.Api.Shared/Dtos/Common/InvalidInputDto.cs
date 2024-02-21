using System.Collections.Generic;

namespace DrugSchedule.Api.Shared.Dtos;

public class InvalidInputDto
{
    public List<string> Messages { get; set; } = new List<string>();
}