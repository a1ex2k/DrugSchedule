namespace DrugSchedule.Api.Services.FileStorage;

public class File
{
    public required Guid Guid { get; set; }

    public required string FileName { get; set; }

    public required DateTime CreatedAt { get; set; }

    public required Stream DataStream { get; set; }
}