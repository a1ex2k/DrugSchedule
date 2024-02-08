namespace DrugSchedule.BusinessLogic.Models;

public class AwaitableFileParams
{
    public required long MaxSize { get; set; }

    public required string[] FileExtensions { get; set; }
}
