using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Services.Models;

public class FileCollection
{
    public required List<DownloadableFile> Files { get; set; }
}