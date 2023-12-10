using System.Collections.Generic;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.StorageContract.Data;

public class MedicamentReleaseFormFilter
{
    public IList<int>? IdFilter { get; set; }

    public StringFilter? NameFilter { get; set; }
}