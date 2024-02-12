namespace DrugSchedule.Services.Models;

public class NewPasswordModel
{
    public required string OldPassword { get; set; }

    public required string NewPassword { get; set; }
}