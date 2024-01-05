using System.ComponentModel.DataAnnotations;

namespace DrugSchedule.BusinessLogic.Auth;

public class TokenModel
{
    public required string AccessToken { get; set; }

    public required string RefreshToken { get; set; }
}