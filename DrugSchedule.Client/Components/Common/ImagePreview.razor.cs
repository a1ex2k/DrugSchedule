using Microsoft.AspNetCore.Components;

namespace DrugSchedule.Client.Components.Common;

public partial class ImagePreview<TImageModel>
{
    [Parameter, EditorRequired] public TImageModel? ImageModel { get; set; }

    [Parameter] public Func<TImageModel?, string?> ThumbnailUrl { get; set; } = default!;

    [Parameter] public Func<TImageModel?, string?> FullImageUrl { get; set; } = default!;

    [Parameter] public Func<TImageModel?, string?> Alt { get; set; } = default!;

    [Parameter, EditorRequired] public string PreviewSize { get; set; } = default!;

    [Parameter] public bool AllowDelete { get; set; }

    [Parameter] public Func<TImageModel?, Task>? OnDelete { get; set; } = default;
}