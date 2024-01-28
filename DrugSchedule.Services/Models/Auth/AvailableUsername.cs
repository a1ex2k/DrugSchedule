using System.ComponentModel.DataAnnotations;

namespace DrugSchedule.BusinessLogic.Models;

public class AvailableUsername
{
    public required string Username { get; set; }

    public required bool IsAvailable { get; set; }

    public required string Comment { get; set; }
}
