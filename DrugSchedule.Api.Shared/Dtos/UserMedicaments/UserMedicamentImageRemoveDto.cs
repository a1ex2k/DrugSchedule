using System;

namespace DrugSchedule.Api.Shared.Dtos;

public class UserMedicamentImageRemoveDto
{
    public long UserMedicamentId { get; set; }
    public Guid FileGuid { get; set; }
}