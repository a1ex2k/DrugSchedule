namespace DrugSchedule.BusinessLogic.Models;

public class DownloadableFile
{
    public required Guid Guid { get; set; }

    public required string Name { get; set; }

    public required string MediaType { get; set; }

    public required long Size { get; set; }

    public required string DownloadUrl { get; set; }
}