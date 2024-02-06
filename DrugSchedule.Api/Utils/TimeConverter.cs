namespace DrugSchedule.Api.Utils;

public static class TimeConverter
{
    public static long ToUnixTime(this DateTime utc)
    {
        return ((DateTimeOffset)utc).ToUnixTimeSeconds();
    }
}