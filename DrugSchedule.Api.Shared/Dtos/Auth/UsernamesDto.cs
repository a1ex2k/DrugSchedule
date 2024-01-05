using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DrugSchedule.Api.Shared.Models;

public class UsernameDto
{
    [Required(ErrorMessage = "Username is required")]
    public string? Username { get; set; }
}