using System.ComponentModel.DataAnnotations;

namespace DrugSchedule.Api.Shared.Dtos;

public class UsernameDto
{
    [Required(ErrorMessage = "Username is required")]
    public required string Username { get; set; }
}