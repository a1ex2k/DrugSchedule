using System.Collections.Generic;

namespace DrugSchedule.StorageContract.Data;

public class MedicamentFilter
{
    public List<int>? IdFilter { get; set; }

    public StringFilter? NameFilter { get; set; }

    public ManufacturerFilter? ManufacturerFilter { get; set; }

    public MedicamentReleaseFormFilter? MedicamentReleaseFormFilter { get; set; }
}