using System.ComponentModel.DataAnnotations;

namespace DrugSchedule.BusinessLogic.Auth;

public class TokenModel
{
    [Required]
    public string? AccessToken { get; set; }

    [Required]
    public string? RefreshToken { get; set; }
}