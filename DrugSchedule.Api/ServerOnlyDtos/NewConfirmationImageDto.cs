using DrugSchedule.Api.Shared.Dtos;

namespace DrugSchedule.Api.ServerOnlyDtos;

public class NewConfirmationImageDto
{
    public required ConfirmationIdDto ConfirmationId { get; set; }

    public required IFormFile FormFile { get; set; }
}