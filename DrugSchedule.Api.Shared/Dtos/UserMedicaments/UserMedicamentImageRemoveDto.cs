#nullable disable
using System;

namespace DrugSchedule.Api.Shared.Dtos;

public partial class UserMedicamentImageRemoveDto
{
    public long MedicamentId { get; set; }
    public FileIdDto ImageId { get; set; }
}