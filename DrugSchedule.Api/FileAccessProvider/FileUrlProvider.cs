using DrugSchedule.BusinessLogic.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace DrugSchedule.Api.FileAccessProvider;

public class FileUrlProvider : IFileUrlProvider
{
    private readonly IUrlHelper _urlHelper;
    private readonly IFileAccessService _accessService;


    public FileUrlProvider(IUrlHelper urlHelper, IFileAccessService accessService)
    {
        _urlHelper = urlHelper;
        _accessService = accessService;
    }

    public string GetPrivateFileUri(Guid fileGuid, CancellationToken cancellationToken = default)
    {
        var accessParams = _accessService.Generate(fileGuid);
        var link = _urlHelper.Action(controller: "Files", action: "Download", 
            values: new
            {
                fileGuid = accessParams.FileGuid,
                accessKey = accessParams.AccessKey,
                expiry = accessParams.ExpiryTime,
                signature = accessParams.Signature
            });
        return link!;
    }

    public string GetPublicFileUri(Guid fileGuid, CancellationToken cancellationToken = default)
    {
        var link = _urlHelper.Action(controller: "Files", action: "Download", values: new { fileGuid = fileGuid });
        return link!;
    }
}