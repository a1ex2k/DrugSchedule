using SixLabors.ImageSharp.Processing.Processors;
using System.IO;

namespace DrugSchedule.Services.Services;

public interface IThumbnailService
{
    Task<MemoryStream?> CreateThumbnail(Stream stream, string mimeType, bool crop, CancellationToken cancellationToken = default);
}