namespace DrugSchedule.Api.ServerOnlyDtos;

public class NewUserMedicamentImageDto
{
    public required long UserMedicamentId { get; set; }

    public required IFormFile FormFile { get; set; }
}