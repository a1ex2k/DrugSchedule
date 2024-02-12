﻿using DrugSchedule.BusinessLogic.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DrugSchedule.Api.FileAccessProvider;

public class FileUrlProvider : IFileUrlProvider
{
    private readonly IFileAccessService _accessService;   
    private readonly LinkGenerator _linkGenerator;


    public FileUrlProvider(LinkGenerator linkGenerator, IHttpContextAccessor contextAccessor, IFileAccessService accessService)
    {
        _linkGenerator = linkGenerator;
        _accessService = accessService;
    }

    public string GetPrivateFileUri(Guid fileGuid, CancellationToken cancellationToken = default)
    {
        var accessParams = _accessService.Generate(fileGuid);
        var link = _linkGenerator.GetPathByAction(controller: "Files", action: "DownloadPrivate", 
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
        var link = _linkGenerator.GetPathByAction(
            action: "DownloadPublic", 
            controller: "Files",
            values: new { fileGuid = fileGuid.ToString() }
        );
        return link!;
    }

    public string GetPrivateFileThumbnailUri(Guid fileGuid, CancellationToken cancellationToken = default)
    {
        var accessParams = _accessService.Generate(fileGuid);
        var link = _linkGenerator.GetPathByAction(controller: "Files", action: "DownloadPrivate",
            values: new
            {
                fileGuid = accessParams.FileGuid,
                accessKey = accessParams.AccessKey,
                expiry = accessParams.ExpiryTime,
                signature = accessParams.Signature,
                thumb = true
            });
        return link!;
    }

    public string GetPublicFileThumbnailUri(Guid fileGuid, CancellationToken cancellationToken = default)
    {
        var link = _linkGenerator.GetPathByAction(
            action: "DownloadPublic",
            controller: "Files",
            values: new { fileGuid = fileGuid.ToString(), thumb = true }
        );
        return link!;
    }
}