using System.Collections.Generic;

namespace DrugSchedule.Api.Shared.Models;

public class StatusResponseDto
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public List<string>? Errors { get; set; }
}