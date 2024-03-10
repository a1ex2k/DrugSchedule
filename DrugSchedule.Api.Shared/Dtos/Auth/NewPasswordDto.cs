using System.ComponentModel.DataAnnotations;

namespace DrugSchedule.Api.Shared.Dtos;

public class NewPasswordDto
{
    [Required]
    public string OldPassword { get; set; } = default!;

    [Required]
    public string NewPassword { get; set; } = default!;
}