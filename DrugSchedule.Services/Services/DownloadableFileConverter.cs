using DrugSchedule.BusinessLogic.Models;
using DrugSchedule.BusinessLogic.Services.Abstractions;

namespace DrugSchedule.BusinessLogic.Services;

public class DownloadableFileConverter : IDownloadableFileConverter
{
    private readonly IFileUrlProvider _fileUrlProvider;

    public DownloadableFileConverter(IFileUrlProvider fileUrlProvider)
    {
        _fileUrlProvider = fileUrlProvider;
    }

    private DownloadableFile Convert(FileInfo fileInfo, bool isPublic)
    {
        var model = new DownloadableFile
        {
            Guid = fileInfo.Guid,
            Name = $"{fileInfo.OriginalName}.{fileInfo.FileExtension}",
            MediaType = fileInfo.MediaType,
            Size = fileInfo.Size,
            DownloadUrl = isPublic
                ? _fileUrlProvider.GetPublicFileUri(fileInfo.Guid)
                : _fileUrlProvider.GetPrivateFileUri(fileInfo.Guid)
        };
        return model;
    }

    public DownloadableFile ToDownloadableFile(FileInfo fileInfo, bool isPublic)
    {
        return Convert(fileInfo, isPublic);
    }

    public List<DownloadableFile> ToDownloadableFiles(List<FileInfo> fileInfos, bool arePublic)
    {
        return fileInfos.ConvertAll(f => Convert(f, arePublic));
    }
}