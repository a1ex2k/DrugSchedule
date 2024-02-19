using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Api.Utils;

public static class RequestedCollectionSizeLimiter
{
    public static void Limit(this FilterBase filter, int sizeLimit)
    {
        if (filter.Take > sizeLimit)
        {
            filter.Take = sizeLimit;
        }
    }
}