using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Services.Utils;

public static class LimitExtensions
{
    private static SizeLimiterOptions? _limits;
    private static bool _isSet;

    public static void SetLimits(SizeLimiterOptions? options)
    {
        if (_isSet)
        {
            throw new InvalidOperationException("_limits already set");
        }

        _limits = options;
        _isSet = true;
    }

    public static void LimitSimple(this FilterBase filter) =>
        filter.Limit(_limits?.SimpleCollectionMaxSize ?? int.MaxValue);

    public static void LimitExtended(this FilterBase filter) =>
        filter.Limit(_limits?.ExtendedCollectionMaxSize ?? int.MaxValue);

    public static string Limit(this string? str) =>
        str.Limit(_limits?.StringLengthLimit ?? int.MaxValue);

    public static void Limit(this FilterBase filter, int sizeLimit)
    {
        if (filter.Take > sizeLimit)
        {
            filter.Take = sizeLimit;
        }
    }

    public static string Limit(this string? str, int sizeLimit)
    {
        if (str == null) return null!;
        if (str.Length <= sizeLimit) return str;
        return str.Substring(sizeLimit);
    }
}