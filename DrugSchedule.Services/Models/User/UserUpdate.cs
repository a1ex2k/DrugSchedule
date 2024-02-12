using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Services.Models;

public class UserUpdate
{
    public string? RealName { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public Sex? Sex { get; set; }
}