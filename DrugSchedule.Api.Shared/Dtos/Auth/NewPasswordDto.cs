using System.ComponentModel.DataAnnotations;

namespace DrugSchedule.Api.Shared.Dtos;

public class NewPasswordDto
{
    [Required]
    public required string OldPassword { get; set; }

    [Required]

    public required string NewPassword { get; set; }
}