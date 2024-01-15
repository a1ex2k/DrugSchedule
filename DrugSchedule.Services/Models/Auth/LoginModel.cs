using System.ComponentModel.DataAnnotations;

namespace DrugSchedule.BusinessLogic.Models;

public class LoginModel
{
    public required string Username { get; set; }

    public required string Password { get; set; }
}
