using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Utils;

namespace DrugSchedule.Client.Constants;

public static class Numbers
{
    public const int MedicamentLoadCount = 20;
    public const int ManufacturersLoadCount = 10;
}

public static class Lists
{
    public static readonly EnumElement<TimeOfDayDto>[] TimeOfDay = EnumHumanizeExtensions.TimeOfDayToArray();
}