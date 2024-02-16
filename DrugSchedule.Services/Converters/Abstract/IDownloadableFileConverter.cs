using DrugSchedule.Services.Models;

namespace DrugSchedule.Services.Converters;

public interface IDownloadableFileConverter
{
    DownloadableFile? ToFileModel(FileInfo? fileInfo, bool isPublic);

    List<DownloadableFile> ToFilesModels(List<FileInfo>? fileInfos, bool arePublic);

    string? ToThumbLink(FileInfo? fileInfo, bool isPublic, bool originalAsThumb = false);
}