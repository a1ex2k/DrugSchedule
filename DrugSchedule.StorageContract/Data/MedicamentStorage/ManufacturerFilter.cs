using System.Collections.Generic;

namespace DrugSchedule.StorageContract.Data.MedicamentStorage;

public class ManufacturerFilter
{
    public IList<int>? IdFilter { get; set; }

    public IList<StringFilter>? NameFilter { get; set; }
}