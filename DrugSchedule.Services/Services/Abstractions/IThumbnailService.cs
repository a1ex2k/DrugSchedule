using SixLabors.ImageSharp.Processing.Processors;
using System.IO;

namespace DrugSchedule.BusinessLogic.Services;

public interface IThumbnailService
{
    Task<MemoryStream?> CreateJpgThumbnail(Stream stream, string mimeType, bool crop, CancellationToken cancellationToken = default);
}