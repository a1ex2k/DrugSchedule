using DrugSchedule.Client.Services;
using Microsoft.AspNetCore.Components;

namespace DrugSchedule.Client.Components.Common;

public partial class ImagePreview<TImageModel>
{
    private const string NoImageUrl = "img/no-photo.png";

    [Inject] public IUrlService UrlService { get; set; } = default!;

    [Parameter, EditorRequired] public TImageModel? ImageModel { get; set; }

    [Parameter] public Func<TImageModel?, string?>? ThumbnailUrl { get; set; }

    [Parameter] public Func<TImageModel?, string?>? FullImageUrl { get; set; }

    [Parameter] public Func<TImageModel?, string?>? Alt { get; set; }

    [Parameter, EditorRequired] public string PreviewSize { get; set; } = "100px";

    [Parameter] public bool AllowDelete { get; set; }

    [Parameter] public Func<TImageModel?, Task>? OnDelete { get; set; } = default;


    private string? GetAltText() => Alt?.Invoke(ImageModel);
    private string? GetThumbnailUrl()
    {
        var url = ThumbnailUrl?.Invoke(ImageModel) ?? GetFullImageUrl();
        return string.IsNullOrWhiteSpace(url) ? NoImageUrl : UrlService.ToApiServerAbsoluteUrl(url);
    }
    private string? GetFullImageUrl() => UrlService.ToApiServerAbsoluteUrl(FullImageUrl?.Invoke(ImageModel));
}