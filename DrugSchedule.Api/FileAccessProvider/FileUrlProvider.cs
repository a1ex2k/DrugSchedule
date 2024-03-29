using DrugSchedule.Services.Services.Abstractions;

namespace DrugSchedule.Api.FileAccessProvider;

public class FileUrlProvider : IFileUrlProvider
{
    private readonly IFileAccessService _accessService;
    private readonly LinkGenerator _linkGenerator;
    private readonly Uri _baseUri;


    public FileUrlProvider(LinkGenerator linkGenerator, IHttpContextAccessor contextAccessor, IFileAccessService accessService, IConfiguration configuration)
    {
        _linkGenerator = linkGenerator;
        _accessService = accessService;
        _baseUri = new Uri(configuration.GetValue<string>("ApplicationUrl")!);
    }

    public string GetPrivateFileUri(Guid fileGuid, CancellationToken cancellationToken = default)
    {
        var accessParams = _accessService.Generate(fileGuid);
        var link = _linkGenerator.GetPathByAction(controller: "Files", action: "Download",
            values: new
            {
                fileGuid = accessParams.FileGuid,
                accessKey = accessParams.AccessKey,
                expiry = accessParams.ExpiryTime,
                signature = accessParams.Signature
            });
        return GetAbsoluteUrl(link)!;
    }

    public string GetPublicFileUri(Guid fileGuid, CancellationToken cancellationToken = default)
    {
        var link = _linkGenerator.GetPathByAction(
            action: "Download",
            controller: "Files",
            values: new { fileGuid = fileGuid.ToString() }
        );
        return GetAbsoluteUrl(link)!;
    }

    public string GetPrivateFileThumbnailUri(Guid fileGuid, CancellationToken cancellationToken = default)
    {
        var accessParams = _accessService.Generate(fileGuid);
        var link = _linkGenerator.GetPathByAction(controller: "Files", action: "Download",
            values: new
            {
                fileGuid = accessParams.FileGuid,
                accessKey = accessParams.AccessKey,
                expiry = accessParams.ExpiryTime,
                signature = accessParams.Signature,
                thumb = true
            });
        return GetAbsoluteUrl(link)!;
    }

    public string GetPublicFileThumbnailUri(Guid fileGuid, CancellationToken cancellationToken = default)
    {
        var link = _linkGenerator.GetPathByAction(
            action: "Download",
            controller: "Files",
            values: new { fileGuid = fileGuid.ToString(), thumb = true }
        );
        return GetAbsoluteUrl(link)!;
    }

    private string? GetAbsoluteUrl(string? relativeUrl)
    {
        Uri.TryCreate(_baseUri, relativeUrl, out var absoluteUrl);
        return absoluteUrl?.ToString();
    }
}