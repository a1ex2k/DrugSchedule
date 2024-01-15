namespace DrugSchedule.BusinessLogic.Models;

public class NewPasswordModel
{
    public required string OldPassword { get; set; }

    public required string NewPassword { get; set; }
}