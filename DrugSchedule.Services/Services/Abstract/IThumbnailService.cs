namespace DrugSchedule.Services.Services;

public interface IThumbnailService
{
    Task<MemoryStream?> CreateThumbnail(Stream stream, string mimeType, CancellationToken cancellationToken = default);
}