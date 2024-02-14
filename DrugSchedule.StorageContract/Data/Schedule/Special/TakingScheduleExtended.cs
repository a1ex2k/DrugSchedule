using System;
using System.Collections.Generic;

namespace DrugSchedule.StorageContract.Data;

public class TakingScheduleExtended
{
    public long Id { get; set; }

    public MedicamentSimple? GlobalMedicament { get; set; }

    public UserMedicamentSimple? UserMedicament { get; set; }

    public string? Information { get; set; }

    public required DateTime CreatedAt { get; set; }

    public required bool Enabled { get; set; }

    public List<ScheduleRepeatPlain> ScheduleRepeats { get; set; } = new();

    public List<ScheduleShare> ScheduleShares { get; set; } = new();
}