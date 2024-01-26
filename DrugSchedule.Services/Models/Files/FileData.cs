namespace DrugSchedule.BusinessLogic.Models;

public class FileData
{
    public required FileInfo FileInfo { get; set; }

    public required Stream Stream { get; set; }
}