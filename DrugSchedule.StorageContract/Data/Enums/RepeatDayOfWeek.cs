﻿using System;

namespace DrugSchedule.StorageContract.Data;

[Flags]
public enum RepeatDayOfWeek : byte
{
    Sunday = 1 << 0,
    Monday = 1 << 1,
    Tuesday = 1 << 2,
    Wednesday = 1 << 3,
    Thursday = 1 << 4,
    Friday = 1 << 5,
    Saturday = 1 << 6
}