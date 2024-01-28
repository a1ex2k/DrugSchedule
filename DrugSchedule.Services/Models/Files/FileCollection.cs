using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.BusinessLogic.Models;

public class FileCollection
{
    public required List<DownloadableFile> Files { get; set; }
}