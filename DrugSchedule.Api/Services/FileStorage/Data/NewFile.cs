namespace DrugSchedule.Api.Services.FileStorage;

public class NewFile
{
    public required string FileName { get; set; }

    public required Stream DataStream { get; set; }
}