namespace DrugSchedule.StorageContract.Data;

public class TakingScheduleUpdateFlags
{
    public bool GlobalMedicamentId { get; set; }

    public bool UserMedicamentId { get; set; }

    public bool Information { get; set; }

    public bool Enabled { get; set; }
}