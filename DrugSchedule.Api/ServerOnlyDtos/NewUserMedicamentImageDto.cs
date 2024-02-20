﻿using DrugSchedule.Api.Shared.Dtos;

namespace DrugSchedule.Api.ServerOnlyDtos;

public class NewUserMedicamentImageDto
{
    public required UserMedicamentIdDto UserMedicamentId { get; set; }

    public required IFormFile FormFile { get; set; }
}