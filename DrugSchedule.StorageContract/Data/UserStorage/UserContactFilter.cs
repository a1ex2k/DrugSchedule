using System.Collections.Generic;

namespace DrugSchedule.StorageContract.Data;

public class UserContactFilter : FilterBase
{
    public List<long>? ContactProfileIdFilter { get; set; }

    public StringFilter? ContactNameFilter { get; set; }
}