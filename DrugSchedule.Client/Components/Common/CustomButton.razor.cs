using Blazorise;
using Microsoft.AspNetCore.Components;

namespace DrugSchedule.Client.Components.Common;

public partial class CustomButton
{
    [Parameter] public Blazorise.Size Size { get; set; } = Blazorise.Size.Small;

    [Parameter] public string Class { get; set; } = default!;

    [Parameter] public string Style { get; set; } = default!;

    [Parameter] public bool Outline { get; set; } = default!;

    [Parameter] public string Width { get; set; } = default!;

    [Parameter] public bool Displayable { get; set; } = true;

    [Parameter] public string Text { get; set; } = default!;

    [Parameter] public EventCallback Clicked { get; set; }

    [Parameter] public string Icon { get; set; } = default!;

    [Parameter] public Blazorise.Color Color { get; set; } = Blazorise.Color.Primary;

    [Parameter] public bool Hidden { get; set; } = false;

    [Parameter] public ButtonType Type { get; set; } = ButtonType.Button;
}