using DrugSchedule.StorageContract.Data.Common;
using DrugSchedule.StorageContract.Data.MedicamentStorage.Filters;
using System.Collections.Generic;

namespace DrugSchedule.StorageContract.Data.MedicamentStorage;

public class MedicamentFilter
{
    public IList<int>? IdFilter { get; set; }

    public IList<StringFilter>? NameFilter { get; set; }

    public IList<ManufacturerFilter>? ManufacturerFilter { get; set; }

    public IList<MedicamentReleaseFormFilter>? MedicamentReleaseFormFilter { get; set; }
}