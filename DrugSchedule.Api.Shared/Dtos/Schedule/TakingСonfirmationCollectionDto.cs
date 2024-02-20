using System.Collections.Generic;
using DrugSchedule.Api.Shared.Dtos;

namespace DrugSchedule.Api.Shared.Dtos;

public class TakingСonfirmationCollectionDto
{
    public List<TakingСonfirmationDto> Confirmations { get; set; } = new();
}