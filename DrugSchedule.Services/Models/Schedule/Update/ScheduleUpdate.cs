namespace DrugSchedule.Services.Models;


public class ScheduleUpdate
{
    public long Id { get; set; }

    public int? GlobalMedicamentId { get; set; }

    public long? UserMedicamentId { get; set; }

    public string? Information { get; set; }

    public required bool Enabled { get; set; }
}