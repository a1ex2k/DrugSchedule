using DrugSchedule.Api.Shared.Dtos;
using Microsoft.AspNetCore.Components;

namespace DrugSchedule.Client.Components;

public partial class ContactExtended
{
    [Parameter, EditorRequired] public UserContactDto Contact { get; set; } = default!;
}