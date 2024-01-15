namespace DrugSchedule.Storage.Extensions;

public static class IListExtensions
{
    public static bool IsNullOrEmpty<TItem>(this IList<TItem>? list)
    {
        return list is null || list.Count == 0;
    }
}