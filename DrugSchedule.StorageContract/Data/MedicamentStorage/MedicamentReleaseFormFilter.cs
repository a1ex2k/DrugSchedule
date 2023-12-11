using System.Collections.Generic;

namespace DrugSchedule.StorageContract.Data;

public class MedicamentReleaseFormFilter
{
    public IList<int>? IdFilter { get; set; }

    public StringFilter? NameFilter { get; set; }
}