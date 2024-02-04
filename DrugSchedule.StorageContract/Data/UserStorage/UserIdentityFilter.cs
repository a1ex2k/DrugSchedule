using System;
using System.Collections.Generic;

namespace DrugSchedule.StorageContract.Data;

public class UserIdentityFilter : FilterBase
{
    public List<Guid>? GuidsFilter { get; set; }

    public StringFilter? UsernameFilter { get; set; }
}