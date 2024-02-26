using DrugSchedule.Api.Shared.Converters;
using System;
using System.Text.Json.Serialization;

namespace DrugSchedule.Api.Shared.Dtos;

[Flags]
[JsonConverter(typeof(RepeatDayOfWeekDtoConverter))]
public enum RepeatDayOfWeekDto : byte
{
    Sunday = 1 << 0,
    Monday = 1 << 1,
    Tuesday = 1 << 2,
    Wednesday = 1 << 3,
    Thursday = 1 << 4,
    Friday = 1 << 5,
    Saturday = 1 << 6
}