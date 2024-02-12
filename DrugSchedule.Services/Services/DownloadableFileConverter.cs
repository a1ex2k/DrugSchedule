using DrugSchedule.Services.Models;
using DrugSchedule.Services.Services.Abstractions;

namespace DrugSchedule.Services.Services;

public class DownloadableFileConverter : IDownloadableFileConverter
{
    private readonly IFileUrlProvider _fileUrlProvider;

    public DownloadableFileConverter(IFileUrlProvider fileUrlProvider)
    {
        _fileUrlProvider = fileUrlProvider;
    }

    private DownloadableFile ToFileModelInternal(FileInfo fileInfo, bool isPublic)
    {
        var model = new DownloadableFile
        {
            Guid = fileInfo.Guid,
            Name = $"{fileInfo.OriginalName}.{fileInfo.FileExtension}",
            MediaType = fileInfo.MediaType,
            Size = fileInfo.Size,
            DownloadUrl = isPublic
                ? _fileUrlProvider.GetPublicFileUri(fileInfo.Guid)
                : _fileUrlProvider.GetPrivateFileUri(fileInfo.Guid),
            ThumbnailUrl = ToThumbLink(fileInfo, isPublic, false)
        };
        return model;
    }


    public DownloadableFile? ToFileModel(FileInfo? fileInfo, bool isPublic)
    {
        return fileInfo == null ? null : ToFileModelInternal(fileInfo, isPublic);
    }


    public List<DownloadableFile> ToFilesModels(List<FileInfo>? fileInfos, bool arePublic)
    {
        if (fileInfos == null || fileInfos.Count == 0)
        {
            return new();
        }

        return fileInfos.ConvertAll(e => ToFileModelInternal(e, arePublic));
    }


    public string? ToThumbLink(FileInfo? fileInfo, bool isPublic, bool originalAsThumb = false)
    {
        if (fileInfo is null)
        {
            return null;
        }

        if (originalAsThumb)
        {
            return isPublic
                ? _fileUrlProvider.GetPublicFileUri(fileInfo.Guid)
                : _fileUrlProvider.GetPrivateFileUri(fileInfo.Guid);
        }

        if (!fileInfo.HasThumbnail)
        {
            return null;
        }

        return isPublic
            ? _fileUrlProvider.GetPublicFileThumbnailUri(fileInfo.Guid)
            : _fileUrlProvider.GetPrivateFileThumbnailUri(fileInfo.Guid);
    }
}