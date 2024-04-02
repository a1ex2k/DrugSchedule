using Microsoft.AspNetCore.Components;

namespace DrugSchedule.Client.Components.Common;

public partial class ImagePreview<TImageModel>
{
    [Parameter, EditorRequired] public TImageModel? ImageModel { get; set; }

    [Parameter] public Func<TImageModel?, string?>? ThumbnailUrl { get; set; }

    [Parameter] public Func<TImageModel?, string?>? FullImageUrl { get; set; }

    [Parameter] public Func<TImageModel?, string?>? Alt { get; set; }

    [Parameter, EditorRequired] public string PreviewSize { get; set; } = "100px";

    [Parameter] public bool AllowDelete { get; set; }

    [Parameter] public Func<TImageModel?, Task>? OnDelete { get; set; } = default;


    private string? GetAltText() => Alt?.Invoke(ImageModel);
    private string? GetThumbnailUrl() => ThumbnailUrl?.Invoke(ImageModel);
    private string? GetFullImageUrl() => FullImageUrl?.Invoke(ImageModel);

}