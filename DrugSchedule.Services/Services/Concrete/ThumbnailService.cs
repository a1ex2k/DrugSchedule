using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace DrugSchedule.Services.Services;

public class ThumbnailService : IThumbnailService
{
    private const int MaxSideSize = 256;

    public async Task<MemoryStream?> CreateThumbnail(Stream sourceStream, string mimeType, CancellationToken cancellationToken = default)
    {
        if (!mimeType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        using var image = await TryCreateImageAsync(sourceStream, cancellationToken);
        if (image == null)
        {
            return null;
        }

        Resize(image, MaxSideSize);
        return await SaveAsync(image, cancellationToken);
    }

    private async Task<Image?> TryCreateImageAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(stream);

        try
        {
            stream.Position = 0;
            var image = await Image.LoadAsync(stream, cancellationToken);
            return image;
        }
        catch
        {
            return null;
        }
    }

    private async Task<MemoryStream> SaveAsync(Image image, CancellationToken cancellationToken = default)
    {
        var stream = new MemoryStream();
        await image.SaveAsync(stream, new JpegEncoder(), cancellationToken);
        return stream;
    }

    private void Resize(Image image, int maxDimensionSize)
    {
        image.Mutate(x => x.Resize(new ResizeOptions
        {
            Size = new Size(maxDimensionSize, maxDimensionSize),
            Mode = ResizeMode.Max,
        }));
    }
}