using Blazorise.RichTextEdit;
using Microsoft.AspNetCore.Components;

namespace DrugSchedule.Client.Components.Common;

public partial class RichEditor
{
    [Parameter] public bool UseBubbleTheme { get; set; } = false;

    [Parameter] public bool ReadOnly { get; set; } = false;

    [Parameter] public bool Multiline { get; set; } = false;

    [Parameter] public bool HideElements { get; set; } = false;

    protected RichTextEdit richTextEditRef;
    protected string contentAsHtml;
    private bool _allowSet = true;

    [Parameter]
    public string Content
    {
        get => contentAsHtml;
        set
        {
            if (contentAsHtml == value) return;
            contentAsHtml = value;
            ContentChanged.InvokeAsync(value);
        }
    }

    [Parameter] public EventCallback<string> ContentChanged { get; set; }

    [Parameter] public RenderFragment AdditionalButtons { get; set; }

    public async Task OnContentChanged()
    {
        var html = await richTextEditRef.GetHtmlAsync();
        Content = html;
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            richTextEditRef.SetHtmlAsync(Content);
            _allowSet = false;
        }

        base.OnAfterRender(firstRender);
    }
}