using System.ComponentModel.DataAnnotations;

namespace DrugSchedule.BusinessLogic.Models;

public class RegisterModel
{
    public required string Username { get; set; }

    public required string Email { get; set; }

    public required string Password { get; set; }
}