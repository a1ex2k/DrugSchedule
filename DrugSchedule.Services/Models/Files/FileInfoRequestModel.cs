namespace DrugSchedule.BusinessLogic.Models;

public class FileRequestModel
{
    public required Guid FileGuid { get; set; }

    public required long DownloadId { get; set; }
}