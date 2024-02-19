using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Services.Utils;

public static class LimitExtensions
{
    public static void Limit(this FilterBase filter, int sizeLimit)
    {
        if (filter.Take > sizeLimit)
        {
            filter.Take = sizeLimit;
        }
    }

    public static string Limit(string? str, int sizeLimit)
    {
        if (str == null) return null;
        if (str.Length <= sizeLimit) return str;
        return str.Substring(sizeLimit);
    }
}