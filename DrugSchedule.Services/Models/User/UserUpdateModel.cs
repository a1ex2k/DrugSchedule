using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.BusinessLogic.Models;

public class UserUpdateModel
{
    public string? RealName { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public Sex? Sex { get; set; }
}