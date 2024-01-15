using System.Collections.Generic;

namespace DrugSchedule.StorageContract.Data;

public class MedicamentReleaseFormFilter
{
    public List<int>? IdFilter { get; set; }

    public StringFilter? NameFilter { get; set; }
}