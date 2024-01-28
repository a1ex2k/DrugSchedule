using System;
using System.Collections.Generic;
using DrugSchedule.StorageContract.Data.Common;

namespace DrugSchedule.StorageContract.Data;

public class UserIdentityFilter : FilterBase
{
    public List<Guid>? GuidsFilter { get; set; }

    public StringFilter? UsernameFilter { get; set; }
}