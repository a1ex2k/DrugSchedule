using System.Collections.Generic;
using DrugSchedule.StorageContract.Data.Common;

namespace DrugSchedule.StorageContract.Data.MedicamentStorage.Filters;

public class MedicamentReleaseFormFilter
{
    public IList<int>? IdFilter { get; set; }

    public IList<StringFilter>? NameFilter { get; set; }
}