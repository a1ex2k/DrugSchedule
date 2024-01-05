using System.ComponentModel.DataAnnotations;

namespace DrugSchedule.Api.Shared.Dtos;

public class TokenDto
{
    [Required]
    public string? AccessToken { get; set; }

    [Required]
    public string? RefreshToken { get; set; }
}