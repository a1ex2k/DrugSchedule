namespace DrugSchedule.Api.Services.FileStorage;

public class RemovedFile
{
    public required Guid Guid { get; set; }

    public required string FileName { get; set; }
}