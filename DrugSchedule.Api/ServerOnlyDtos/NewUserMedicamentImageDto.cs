using DrugSchedule.Api.Shared.Dtos;
using Microsoft.Extensions.FileProviders;

namespace DrugSchedule.Api.ServerOnlyDtos;

public class NewUserMedicamentImageDto
{
    public required UserMedicamentIdDto UserMedicamentId { get; set; }

    public required IFormFile FormFile { get; set; }
}