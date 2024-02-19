using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Services.Utils;

internal static class DateUtility
{
    public static IEnumerable<DateOnly> GetRange(DateOnly startDate, DateOnly endDate)
    {
        if (endDate < startDate)
            throw new ArgumentException("endDate must be greater than or equal to startDate");

        while (startDate <= endDate)
        {
            yield return startDate;
            startDate = startDate.AddDays(1);
        }
    }

    public static IEnumerable<DateOnly> GetRange(DateOnly startDate, DateOnly endDate, RepeatDayOfWeek dayOfWeek)
    {
        if (endDate < startDate)
            throw new ArgumentException("endDate must be greater than or equal to startDate");

        while (startDate <= endDate)
        {
            if (((byte)dayOfWeek & (byte)startDate.DayOfWeek) == (byte)startDate.DayOfWeek)
            {
                yield return startDate;
            }

            startDate = startDate.AddDays(1);
        }
    }

    public static DateOnly Min(DateOnly date1, DateOnly date2)
    {
        return date1 <= date2 ? date1 : date2;
    }

    public static DateOnly Max(DateOnly date1, DateOnly date2)
    {
        return date1 >= date2 ? date1 : date2;
    }
}