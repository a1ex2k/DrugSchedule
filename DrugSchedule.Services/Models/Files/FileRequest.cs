namespace DrugSchedule.BusinessLogic.Models;

public class FileRequest
{
    public required Guid Guid { get; set; }

    public required string OriginalName { get; set; }
}