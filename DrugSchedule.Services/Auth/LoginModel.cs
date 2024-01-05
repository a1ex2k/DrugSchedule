using System.ComponentModel.DataAnnotations;

namespace DrugSchedule.BusinessLogic.Auth;

public class LoginModel
{
    public required string Username { get; set; }

    public required string Password { get; set; }
}
