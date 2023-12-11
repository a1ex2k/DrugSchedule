namespace DrugSchedule.Api.Models;

public class StatusResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public List<string>? Errors { get; set; }
}