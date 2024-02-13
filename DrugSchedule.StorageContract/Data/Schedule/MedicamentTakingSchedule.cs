using System;
using System.Collections.Generic;

namespace DrugSchedule.StorageContract.Data;

public class TakingScheduleSimple
{
    public long Id { get; set; }

    public UserProfile? UserProfile { get; set; }

    public MedicamentSimple? GlobalMedicament { get; set; }

    public UserMedicamentSimple? UserMedicament { get; set; }

    public string? Information { get; set; }

    public required DateTime CreatedAt { get; set; }

    public required bool Enabled { get; set; }

    public List<Storage.Data.Entities.ScheduleRepeat> RepeatSchedules { get; set; } = new();

    public List<Storage.Data.Entities.ScheduleShare> ScheduleShares { get; set; } = new();
}