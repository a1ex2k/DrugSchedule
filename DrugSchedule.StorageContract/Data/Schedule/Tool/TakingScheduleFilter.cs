using System;
using System.Collections.Generic;

namespace DrugSchedule.StorageContract.Data;

public class TakingScheduleFilter : FilterBase
{
    public List<long>? IdFilter { get; set; }

    public List<int>? GlobalMedicamentIdFilter { get; set; }

    public List<long>? UserMedicamentIdFilter { get; set; }

    public bool? EnabledFilter { get; set; }
}