﻿using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace DrugSchedule.BusinessLogic.Services;

public class ThumbnailService : IThumbnailService
{
    private const int MaxSideSize = 256;

    public async Task<MemoryStream?> CreateJpgThumbnail(Stream sourceStream, string mimeType, bool crop, CancellationToken cancellationToken = default)
    {
        if (!mimeType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        using var image = await TryCreateAsync(sourceStream, cancellationToken);
        if (image == null)
        {
            return null;
        }

        Resize(image, MaxSideSize, crop);
        return await SaveAsync(image, cancellationToken);
    }

    private async Task<Image?> TryCreateAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(stream);

        try
        {
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

    private void Resize(Image image, int maxDimensionSize, bool crop)
    {
        image.Mutate(x => x.Resize(new ResizeOptions
        {
            Size = new Size(maxDimensionSize, maxDimensionSize),
            Mode = crop ? ResizeMode.Min : ResizeMode.Max,
        }));
    }
}