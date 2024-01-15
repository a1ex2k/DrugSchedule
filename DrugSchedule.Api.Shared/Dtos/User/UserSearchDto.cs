using System.ComponentModel.DataAnnotations;

namespace DrugSchedule.Api.Shared.Dtos;

public class UserSearchDto
{
    [Required]
    public required string UsernameSubstring { get; set; }
}