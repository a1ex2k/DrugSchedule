using DrugSchedule.BusinessLogic.Models;

namespace DrugSchedule.BusinessLogic.Services;

public interface IDownloadableFileConverter
{
    DownloadableFile ToDownloadableFile(FileInfo fileInfo, bool isPublic);

    List<DownloadableFile> ToDownloadableFiles(List<FileInfo> fileInfos, bool arePublic);
}