using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.BusinessLogic.Utils;

internal static class FilterExtensions
{
    private const int MaxCount = 50;
    public static void LimitPaging(this FilterBase filter)
    {
        if (filter.Skip < 0)
        {
            filter.Skip = 0;
        }

        if (filter.Take < 0)
        {
            filter.Take = 0;
        }

        if (filter.Take > MaxCount)
        {
            filter.Take = MaxCount;
        }
    }

}