namespace DrugSchedule.BusinessLogic.Models;

public class FileInfoModel
{
    public required Guid Guid { get; set; }

    public required string NameWithExtension { get; set; }

    public required string MediaType { get; set; }

    public required long Size { get; set; }

    public DateTime CreationTime { get; set; }
}