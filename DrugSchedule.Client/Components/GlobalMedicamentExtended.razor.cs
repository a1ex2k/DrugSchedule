﻿using Blazorise;
using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Constants;
using Microsoft.AspNetCore.Components;

namespace DrugSchedule.Client.Components;

public partial class GlobalMedicamentExtended
{
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;
    [Parameter, EditorRequired] public MedicamentExtendedDto Medicament { get; set; } = default!;


    public IEnumerable<DownloadableFileDto> Images => Medicament?.FileCollection?.Files
        .Where(f => f.MediaType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
        ?? Enumerable.Empty<DownloadableFileDto>();
}