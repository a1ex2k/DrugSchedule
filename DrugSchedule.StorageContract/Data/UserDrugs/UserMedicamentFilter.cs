using System.Collections.Generic;

namespace DrugSchedule.StorageContract.Data;

public class UserMedicamentFilter : FilterBase
{
    public List<long>? IdFilter { get; set; }

    public StringFilter? NameFilter { get; set; }

    public StringFilter? ManufacturerNameFilter { get; set; }

    public StringFilter? ReleaseFormNameFilter { get; set; }
}