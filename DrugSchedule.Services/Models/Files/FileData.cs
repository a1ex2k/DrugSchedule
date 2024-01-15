namespace DrugSchedule.BusinessLogic.Models;

public class FileData
{
    public required FileInfoModel FileInfoModel { get; set; }

    public required Stream Stream { get; set; }
}